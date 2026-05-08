import * as SecureStore from "expo-secure-store";
import { handleUnauthorized, resetUnauthorizedHandlerState } from "./auth";
import {
  CancelTransactionRequest,
  ClientLookup,
  CreateTransactionRequest,
  ItemLookup,
  TransactionForEditDTO,
  WarehouseLookup,
} from "./create-edit-transaction.types";
import {
  GetTransactionsParams,
  PagedResponse,
  TransactionListDTO,
} from "./transactions.types";

//api url loaded from expo env
const API_URL = process.env.EXPO_PUBLIC_API_URL;

if (!API_URL) {
  throw new Error("No EXPO_PUBLIC_API_URL");
}

//types
export type LoginRequest = {
  email: string;
  password: string;
};

export type AuthResponse = {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiresAtUtc: string;
  refreshTokenExpiresAtUtc: string;
  userId: string;
  email: string;
  roles: string[];
};

export type ApiErrorResponse = {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
};

//login function
export async function login(
  email: string,
  password: string,
): Promise<AuthResponse> {
  const body: LoginRequest = {
    email,
    password,
  };

  const url = `${API_URL}/api/auth/login`;

  try {
    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(body),
    });

    if (!response.ok) {
      const errorText = await response.text();
      let message = "Login failed";
      try {
        const errorJson: ApiErrorResponse = JSON.parse(errorText);
        message = errorJson.detail || errorJson.title || message;
      } catch {
        message = errorText || message;
      }
      console.log("LOGIN ERROR BODY:", errorText);
      throw new Error(message);
    }

    const data: AuthResponse = await response.json();
    console.log("LOGIN RESPONSE JSON:", data);

    await SecureStore.setItemAsync("accessToken", data.accessToken);
    await SecureStore.setItemAsync("refreshToken", data.refreshToken);

    resetUnauthorizedHandlerState();
    return data;
  } catch (error) {
    console.log("LOGIN FETCH EXCEPTION:", error);
    throw error;
  }
}

////Business-flow related

//loads paginated transactions list from api
export async function getTransactions(
  params: GetTransactionsParams,
): Promise<PagedResponse<TransactionListDTO>> {
  const query = new URLSearchParams({
    pageNumber: String(params.pageNumber),
    pageSize: String(params.pageSize),
    includeReturns: String(params.includeReturns ?? true),
    includeIssues: String(params.includeIssues ?? true),
    includeTransfers: String(params.includeTransfers ?? true),
    includeAdjustments: String(params.includeAdjustments ?? true),
    searchTerm: String(params.searchTerm ?? ""),
    ...(params.dateFrom ? { dateFrom: params.dateFrom } : {}),
    ...(params.dateTo ? { dateTo: params.dateTo } : {}),
  });
  const response = await authorizedFetch(`/api/user/transactions?${query}`, {
    method: "GET",
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || "Failed to load transactions");
  }

  return await response.json();
}

//gets transaction by ID
export async function getTransactionById(
  transactionId: string,
): Promise<TransactionForEditDTO> {
  const response = await authorizedFetch(
    `/api/user/transactions/${transactionId}`,
    {
      method: "GET",
    },
  );

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || "Failed to load transaction");
  }

  return await response.json();
}

//creates new transaction
export async function createTransaction(
  payload: CreateTransactionRequest,
): Promise<string> {
  const response = await authorizedFetch("/api/user/transactions", {
    method: "POST",
    body: JSON.stringify(payload),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || "Failed to create transaction");
  }

  // API returns GUID of created transaction
  return await response.json();
}

//updates transaction
export async function updateTransaction(
  id: string,
  payload: CreateTransactionRequest,
): Promise<string> {
  const response = await authorizedFetch(`/api/user/transactions/${id}`, {
    method: "PUT",
    body: JSON.stringify(payload),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || "Failed to update transaction");
  }

  // API returns GUID of created transaction
  return await response.json();
}

//cancels transaction
export async function cancelTransaction(
  id: string,
  payload: CancelTransactionRequest,
): Promise<string> {
  const response = await authorizedFetch(
    `/api/user/transactions/${id}/cancel`,
    {
      method: "PATCH",
      body: JSON.stringify(payload),
    },
  );

  if (!response.ok) {
    const errorText = await response.text();
    let message = "Failed to cancel transaction";
    try {
      var parsedError = JSON.parse(errorText);
      message =
        parsedError.detail ||
        parsedError.title ||
        parsedError.message ||
        message;
    } catch {
      message = errorText || message;
    }
    throw new Error(message);
  }

  // API returns GUID of cancelled transaction
  return await response.json();
}

/////Helpers

// access token getter
export async function getAccessToken(): Promise<string | null> {
  return await SecureStore.getItemAsync("accessToken");
}

// refresh token getter
export async function getRefreshToken(): Promise<string | null> {
  return await SecureStore.getItemAsync("refreshToken");
}

//checks if theres refreshtoken in secure store and tries to refresh the access token. returns true when succeeded
export async function tryRefreshToken(): Promise<boolean> {
  try {
    const refreshToken = await getRefreshToken();
    if (!refreshToken) {
      return false;
    }

    const response = await fetch(`${API_URL}/api/auth/refresh`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ refreshToken }),
    });
    if (!response.ok) {
      return false;
    }

    const data: AuthResponse = await response.json();

    await SecureStore.setItemAsync("accessToken", data.accessToken);
    await SecureStore.setItemAsync("refreshToken", data.refreshToken);

    return true;
  } catch {
    return false;
  }
}

//// Wrapper around fetch for endpoints that require authentication.
//It adds the current access token, tries to refresh it on 401/403,
//retries the original request once, and logs the user out if recovery fails.
async function authorizedFetch(url: string, options: RequestInit = {}) {
  let token = await getAccessToken();

  let response = await fetch(`${API_URL}${url}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
      ...(options.headers ?? {}),
    },
  });

  if (response.status !== 401 && response.status !== 403) {
    return response;
  }
  const isTokenRefreshed = await tryRefreshToken();
  if (!isTokenRefreshed) {
    await handleUnauthorized();
    throw new Error("Unauthorized");
  }
  token = await getAccessToken();
  response = await fetch(`${API_URL}${url}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
      ...(options.headers ?? {}),
    },
  });

  if (response.status === 401 || response.status === 403) {
    await handleUnauthorized();
    throw new Error("Unauthorized");
  }

  return response;
}

//// Get lookups for select menus
export async function getWarehousesLookup(): Promise<WarehouseLookup[]> {
  const response = await authorizedFetch("/api/lookups/warehouses");

  if (!response.ok) {
    throw new Error("Failed to load warehouses");
  }

  return await response.json();
}

export async function getClientsLookup(): Promise<ClientLookup[]> {
  const response = await authorizedFetch("/api/lookups/clients");

  if (!response.ok) {
    throw new Error("Failed to load clients");
  }

  return await response.json();
}

export async function getItemsLookup(): Promise<ItemLookup[]> {
  const response = await authorizedFetch("/api/lookups/items");

  if (!response.ok) {
    throw new Error("Failed to load items");
  }

  return await response.json();
}
