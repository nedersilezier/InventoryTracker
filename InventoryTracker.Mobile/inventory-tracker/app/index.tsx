import { router, Stack } from "expo-router";
import { useState, useEffect } from "react";
import {
  Alert,
  Button,
  Text,
  TextInput,
  View,
  ActivityIndicator,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { login, getRefreshToken, tryRefreshToken } from "../lib/api";
import { resetUnauthorizedHandlerState } from '../lib/auth';

export default function Index() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [checkingSession, setCheckingSession] = useState(true);

  useEffect(() => {
    async function restoreSession() {
      try {
        resetUnauthorizedHandlerState();
        const refreshToken = await getRefreshToken();

        if (!refreshToken) {
          return;
        }

        const refreshed = await tryRefreshToken();

        if (refreshed) {
          router.replace("/transactions");
        }
      } finally {
        setCheckingSession(false);
      }
    }

    restoreSession();
  }, []);

  const handleLogin = async () => {
    try {
      setLoading(true);

      await login(email, password);
      resetUnauthorizedHandlerState();
      router.replace("/transactions");
    } catch (error) {
      console.log("LOGIN ERROR:", error);
      const message = error instanceof Error ? error.message : "Unknowk error";

      Alert.alert("Login error", message);
    } finally {
      setLoading(false);
    }
  };

  if (checkingSession) {
    return (
      <SafeAreaView style={{ flex: 1, backgroundColor: "#f9f9ff" }}>
        <View
          style={{ flex: 1, alignItems: "center", justifyContent: "center" }}
        >
          <ActivityIndicator size="large" />
          <Text style={{ marginTop: 12, color: "#5c5f60" }}>
            Checking session...
          </Text>
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={{ flex: 1 }}>
      <Stack.Screen options={{ title: "Login" }} />
      <View
        style={{
          flex: 1,
          justifyContent: "center",
          padding: 24,
          gap: 12,
        }}
      >
        <Text style={{ fontSize: 28, fontWeight: "700" }}>
          Inventory Tracker
        </Text>

        <TextInput
          placeholder="Email"
          value={email}
          onChangeText={setEmail}
          autoCapitalize="none"
          keyboardType="email-address"
          style={{
            borderWidth: 1,
            borderColor: "#ccc",
            borderRadius: 8,
            padding: 12,
          }}
        />

        <TextInput
          placeholder="Password"
          value={password}
          onChangeText={setPassword}
          secureTextEntry
          style={{
            borderWidth: 1,
            borderColor: "#ccc",
            borderRadius: 8,
            padding: 12,
          }}
        />

        <Button
          title={loading ? "Logging in..." : "Login"}
          onPress={handleLogin}
          disabled={loading}
        />
      </View>
    </SafeAreaView>
  );
}
