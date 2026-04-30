import * as SecureStore from 'expo-secure-store';

// callback set from React (layout) to handle navigation
let onUnauthorized: (() => void) | null = null;

//helper to prevent multiple redirections
let isHandling = false;

//allows the layout to register a redirect handler
export function setUnauthorizedHandler(handler: () => void) {
  onUnauthorized = handler;
}

//called when API returns 401, 403
export async function handleUnauthorized() {
  //avoid multiple logouts
  if (isHandling) return;
  isHandling = true;
  //remove tokens
  await SecureStore.deleteItemAsync('accessToken');
  await SecureStore.deleteItemAsync('refreshToken');
  //trigger redirect
  onUnauthorized?.();
}

//reset isHandling after successful re-login
export function resetUnauthorizedHandlerState() {
  isHandling = false;
}