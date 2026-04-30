import { Stack, useRouter } from 'expo-router';
import { useEffect } from 'react';
import { setUnauthorizedHandler } from '../lib/auth';

export default function RootLayout() {
  const router = useRouter();

  useEffect(() => {
    //register global handler for 401, 403 responses, when triggered redirects to login
    setUnauthorizedHandler(() => {
      router.replace('/');
    });
  }, []);

  return (
    <Stack
      screenOptions={{
        headerShown: false,
      }}
    />
  );
}