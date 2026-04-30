import { Ionicons } from '@expo/vector-icons';
import { Pressable, StyleSheet, Text, View } from 'react-native';

// Header props.
// title changes depending on the current create-transaction step.
// onBack is controlled by the parent screen because back behavior depends on wizard state.
type Props = {
  title: string;
  onBack: () => void;
};

// Reusable header for the create transaction flow.
// It does not handle navigation directly; it only calls onBack.
// The parent screen decides whether to go back one wizard step
// or leave the create transaction screen entirely.
export function CreateTransactionHeader({ title, onBack }: Props) {
  return (
    <View style={styles.header}>
      <Pressable style={styles.iconButton} onPress={onBack}>
        <Ionicons name="arrow-back" size={26} color="#041b3c" />
      </Pressable>

      <Text style={styles.headerTitle}>{title}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  header: {
    height: 56,
    paddingHorizontal: 16,
    backgroundColor: '#ffffff',
    borderBottomWidth: 1,
    borderBottomColor: '#dfe1e6',
    flexDirection: 'row',
    alignItems: 'center',
  },
  iconButton: {
    width: 44,
    height: 44,
    alignItems: 'center',
    justifyContent: 'center',
  },
  headerTitle: {
    fontSize: 22,
    fontWeight: '700',
    color: '#041b3c',
    marginLeft: 8,
  },
});