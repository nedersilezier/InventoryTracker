import { Stack, useRouter } from 'expo-router';
import { useEffect } from 'react';
import { setUnauthorizedHandler } from '../lib/auth';

export default function RootLayout() {
  const router = useRouter();

  useEffect(() => {
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