import { StyleSheet, Text, TextInput, View } from 'react-native';


// Props for a reusable text input field
// used for simple form fields like reference number, notes or quantity
type Props = {
  label: string;
  value: string;
  placeholder?: string;
  multiline?: boolean;
  keyboardType?: 'default' | 'numeric';
  onChangeText: (value: string) => void;
};

// Reusable labeled text field
export function TextField({
  label,
  value,
  placeholder,
  multiline,
  keyboardType,
  onChangeText,
}: Props) {
  return (
    <View style={styles.field}>
      <Text style={styles.fieldLabel}>{label}</Text>

      <TextInput
        value={value}
        placeholder={placeholder}
        multiline={multiline}
        keyboardType={keyboardType}
        onChangeText={onChangeText}
        style={[styles.textInput, multiline && styles.textArea]}
      />
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
  textInput: {
    minHeight: 50,
    borderRadius: 12,
    borderWidth: 1,
    borderColor: '#c3c6d6',
    backgroundColor: '#ffffff',
    paddingHorizontal: 14,
    fontSize: 16,
    color: '#041b3c',
  },
  textArea: {
    minHeight: 96,
    paddingTop: 12,
    textAlignVertical: 'top',
  },
});