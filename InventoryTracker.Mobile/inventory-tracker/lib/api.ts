import * as SecureStore from 'expo-secure-store';
import { handleUnauthorized } from './auth';
import { PagedResponse, TransactionListDTO, GetTransactionsParams } from './transactions.types';

const API_URL = process.env.EXPO_PUBLIC_API_URL;

if (!API_URL) {
  throw new Error('No EXPO_PUBLIC_API_URL');
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
export async function login(email: string, password: string): Promise<AuthResponse> {
  const body: LoginRequest = {
    email,
    password,
  };

  const url = `${API_URL}/api/auth/login`;

  try {
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(body),
    });

    if (!response.ok) {
      const errorText = await response.text();
      let message = 'Login failed';
      try{
        const errorJson: ApiErrorResponse = JSON.parse(errorText);
        message = errorJson.detail || errorJson.title || message;
      }
      catch{
        message = errorText || message;
      }
      console.log('LOGIN ERROR BODY:', errorText);
      throw new Error(message);
    }

    const data: AuthResponse = await response.json();
    console.log('LOGIN RESPONSE JSON:', data);

    await SecureStore.setItemAsync('accessToken', data.accessToken);
    await SecureStore.setItemAsync('refreshToken', data.refreshToken);

    return data;
  } catch (error) {
    console.log('LOGIN FETCH EXCEPTION:', error);
    throw error;
  }
}


// access token getter
export async function getAccessToken(): Promise<string | null> {
  return await SecureStore.getItemAsync('accessToken');
}

// refresh token getter
export async function getRefreshToken(): Promise<string | null> {
  return await SecureStore.getItemAsync('refreshToken');
}

//get all transactions function
export async function getTransactions(params: GetTransactionsParams): Promise<PagedResponse<TransactionListDTO>> {
  //get access token
  const token = await getAccessToken();

//build query
const query = new URLSearchParams({
  pageNumber: String(params.pageNumber),
  pageSize: String(params.pageSize),
  includeReturns: String(params.includeReturns ?? true),
  includeIssues: String(params.includeIssues ?? true),
  includeTransfers: String(params.includeTransfers ?? true),
  includeAdjustments: String(params.includeAdjustments ?? true),
  ...(params.dateFrom && { dateFrom: params.dateFrom }),
  ...(params.dateTo && { dateTo: params.dateTo }),
});
//execute request
  const response = await fetch(`${API_URL}/api/user/transactions?${query}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    },
  });
//handle failures
  if (!response.ok) {
    if (response.status === 401 || response.status === 403) {
      await handleUnauthorized();
      throw new Error('Unauthorized');
    }
    const errorText = await response.text();
    throw new Error(errorText || 'Failed to load transactions');
  }

  return await response.json();
}