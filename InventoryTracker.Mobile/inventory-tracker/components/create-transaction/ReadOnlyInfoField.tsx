import { Ionicons } from '@expo/vector-icons';
import { StyleSheet, Text, View } from 'react-native';

// Props for a read-only field
type Props = {
  label: string;
  value: string;
  helperText?: string;
};

// Displays a value in an input-like container, but without TextInput.
// Example: transaction date in mobile flow is assigned automatically,
// so the user can see it but cannot edit it.
export function ReadOnlyInfoField({ label, value, helperText }: Props) {
  return (
    <View style={styles.field}>
      <Text style={styles.fieldLabel}>{label}</Text>

      <View style={styles.readOnlyField}>
        <Text style={styles.readOnlyValue}>{value}</Text>
      </View>

      {helperText ? <Text style={styles.helperText}>{helperText}</Text> : null}
    </View>
  );
}

const styles = StyleSheet.create({
  field: {
    gap: 8,
    marginBottom: 14,
  },
  fieldLabel: {
    fontSize: 12,
    fontWeight: '700',
    color: '#434654',
    textTransform: 'uppercase',
    letterSpacing: 0.6,
  },
  readOnlyField: {
    minHeight: 50,
    borderRadius: 12,
    borderWidth: 1,
    borderColor: '#dfe1e6',
    backgroundColor: '#f4f5f7',
    paddingHorizontal: 14,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
  },
  readOnlyValue: {
    fontSize: 16,
    color: '#041b3c',
    fontWeight: '600',
  },
  helperText: {
    fontSize: 13,
    color: '#5c5f60',
    lineHeight: 18,
  },
});