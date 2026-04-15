import * as SecureStore from 'expo-secure-store';

const API_URL = process.env.EXPO_PUBLIC_API_URL;

if (!API_URL) {
  throw new Error('No EXPO_PUBLIC_API_URL');
}

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

export type MeResponse = {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  roles: string[];
};
export async function login(email: string, password: string): Promise<AuthResponse> {
  const body: LoginRequest = {
    email,
    password,
  };

  const url = `${API_URL}/api/auth/login`;

  console.log('LOGIN URL:', url);
  console.log('LOGIN BODY:', body);

  try {
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(body),
    });

    console.log('LOGIN STATUS:', response.status);
    console.log('LOGIN OK?:', response.ok);

    if (!response.ok) {
      const errorText = await response.text();
      console.log('LOGIN ERROR BODY:', errorText);
      throw new Error(errorText || 'Login failed');
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

// protected endpoint test
export async function getCurrentUser():Promise<MeResponse> {
  const token = await getAccessToken();

  const response = await fetch(`${API_URL}/api/auth/me`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || 'Failed to load the user');
  }

  return await response.json();
}