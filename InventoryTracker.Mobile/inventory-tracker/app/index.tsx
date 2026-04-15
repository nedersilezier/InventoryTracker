import { useState } from 'react';
import { Alert, Button, Text, TextInput, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { getCurrentUser, login } from '../lib/api';

export default function Index() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);

  const handleLogin = async () => {
    try {
      setLoading(true);

      const result = await login(email, password);

      console.log('LOGIN RESULT:', result);

      Alert.alert(
        'Login success',
        `Loged in as ${result.email}`
      );

      const me = await getCurrentUser();
      console.log('CURRENT USER:', me);
      Alert.alert(
        'Me endpoint success',
        `Loged in as ${me.email} \nName: ${me.firstName} \nLast name: ${me.lastName}`
      );
      
    } catch (error) {
      console.log('LOGIN ERROR:', error);
      const message =
        error instanceof Error ? error.message : 'Unknowk error';

      Alert.alert('Login error: ', message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <SafeAreaView style={{ flex: 1 }}>
      <View
        style={{
          flex: 1,
          justifyContent: 'center',
          padding: 24,
          gap: 12,
        }}
      >
        <Text style={{ fontSize: 28, fontWeight: '700' }}>
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
            borderColor: '#ccc',
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
            borderColor: '#ccc',
            borderRadius: 8,
            padding: 12,
          }}
        />

        <Button
          title={loading ? 'Logging in...' : 'Login'}
          onPress={handleLogin}
          disabled={loading}
        />
      </View>
    </SafeAreaView>
  );
}