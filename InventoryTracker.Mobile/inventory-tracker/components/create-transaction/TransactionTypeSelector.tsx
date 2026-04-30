import { Pressable, StyleSheet, Text, View } from 'react-native';
import { TransactionTypeKey } from '../../lib/create-transaction.types';
import { SectionTitle } from './SectionTitle';


// Props for the transaction type selector
// value is the currently selected type
// onChange is handled by the parent screen
type Props = {
  value: TransactionTypeKey;
  onChange: (type: TransactionTypeKey) => void;
};

// Displays available transaction types as buttons
export function TransactionTypeSelector({ value, onChange }: Props) {
  return (
    <>
      <SectionTitle title="Transaction Type" />

      <View style={styles.typeGrid}>
        <TypeButton
          label="Adjustment"
          active={value === 'adjustment'}
          onPress={() => onChange('adjustment')}
        />
        <TypeButton
          label="Issue"
          active={value === 'issue'}
          onPress={() => onChange('issue')}
        />
        <TypeButton
          label="Return"
          active={value === 'return'}
          onPress={() => onChange('return')}
        />
        <TypeButton
          label="Transfer"
          active={value === 'transfer'}
          onPress={() => onChange('transfer')}
        />
      </View>
    </>
  );
}

// Single transaction type button
// styling changes when active
function TypeButton({
  label,
  active,
  onPress,
}: {
  label: string;
  active: boolean;
  onPress: () => void;
}) {
  return (
    <Pressable style={[styles.typeButton, active && styles.typeButtonActive]} onPress={onPress}>
      <Text style={[styles.typeButtonText, active && styles.typeButtonTextActive]}>
        {label}
      </Text>
    </Pressable>
  );
}

const styles = StyleSheet.create({
  typeGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 10,
  },
  typeButton: {
    width: '48%',
    minHeight: 64,
    borderRadius: 14,
    borderWidth: 1,
    borderColor: '#c3c6d6',
    backgroundColor: '#ffffff',
    alignItems: 'center',
    justifyContent: 'center',
    padding: 12,
  },
  typeButtonActive: {
    backgroundColor: '#0052cc',
    borderColor: '#0052cc',
  },
  typeButtonText: {
    fontSize: 15,
    fontWeight: '700',
    color: '#041b3c',
  },
  typeButtonTextActive: {
    color: '#ffffff',
  },
});