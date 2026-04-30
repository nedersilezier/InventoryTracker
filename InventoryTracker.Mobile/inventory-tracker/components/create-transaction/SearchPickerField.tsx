import { Ionicons } from '@expo/vector-icons';
import { Pressable, StyleSheet, Text, View } from 'react-native';

// Props for a form field that opens the shared search picker modal
// used for long lists like clients, warehouses and items
type Props = {
  label: string;
  valueLabel?: string;
  placeholder: string;
  onPress: () => void;
};

// Input-like field used to select a value from a searchable modal
// it displays the selected value, but does not edit text directly
// tapping the field delegates selection to the parent via onPress.
export function SearchPickerField({
  label,
  valueLabel,
  placeholder,
  onPress,
}: Props) {
  return (
    <View style={styles.field}>
      <Text style={styles.fieldLabel}>{label}</Text>

      <Pressable style={styles.pickerField} onPress={onPress}>
        <Text
          style={[
            styles.pickerFieldText,
            
            // Use muted styling when no value is selected yet.
            !valueLabel && styles.pickerFieldPlaceholder,
          ]}
          numberOfLines={1}
        >
          {valueLabel || placeholder}
        </Text>

        <Ionicons name="chevron-forward" size={20} color="#737685" />
      </Pressable>
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
  pickerField: {
    minHeight: 50,
    borderRadius: 12,
    borderWidth: 1,
    borderColor: '#c3c6d6',
    backgroundColor: '#ffffff',
    paddingHorizontal: 14,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    gap: 12,
  },
  pickerFieldText: {
    flex: 1,
    fontSize: 16,
    color: '#041b3c',
    fontWeight: '600',
  },
  pickerFieldPlaceholder: {
    color: '#737685',
    fontWeight: '400',
  },
});