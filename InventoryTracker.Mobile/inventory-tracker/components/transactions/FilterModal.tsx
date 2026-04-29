import { Pressable, StyleSheet, Text, View } from 'react-native';
import { TransactionFilters } from '../../lib/transactions.types';
import { FilterSwitch } from './FilterSwitch';

type Props = {
  visible: boolean;
  filters: TransactionFilters;
  onChange: (filters: TransactionFilters) => void;
  onApply: () => void;
  onClose: () => void;
};

export function FilterModal({ visible, filters, onChange, onApply, onClose, }: Props){
  if (!visible) return null;

  return (
    <View style={styles.overlay}>
      <View style={styles.modal}>
        <Text style={styles.title}>Filters</Text>

        <FilterSwitch
          label="Returns"
          value={filters.includeReturns}
          onChange={(v) => onChange({ ...filters, includeReturns: v })}
        />

        <FilterSwitch
          label="Issues"
          value={filters.includeIssues}
          onChange={(v) => onChange({ ...filters, includeIssues: v })}
        />

        <FilterSwitch
          label="Transfers"
          value={filters.includeTransfers}
          onChange={(v) => onChange({ ...filters, includeTransfers: v })}
        />

        <FilterSwitch
          label="Adjustments"
          value={filters.includeAdjustments}
          onChange={(v) => onChange({ ...filters, includeAdjustments: v })}
        />

        <View style={{ height: 16 }} />

        <Pressable style={styles.applyButton} onPress={onApply}>
          <Text style={styles.applyButtonText}>Apply filters</Text>
        </Pressable>

        <Pressable onPress={onClose}>
          <Text style={styles.cancelText}>Cancel</Text>
        </Pressable>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  overlay: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    backgroundColor: 'rgba(0,0,0,0.4)',
    justifyContent: 'flex-end',
  },
  modal: {
    backgroundColor: 'white',
    padding: 20,
    borderTopLeftRadius: 16,
    borderTopRightRadius: 16,
  },
  title: {
    fontSize: 18,
    fontWeight: '700',
    marginBottom: 16,
    color: '#041b3c',
  },
  applyButton: {
    backgroundColor: '#0052cc',
    padding: 14,
    borderRadius: 12,
  },
  applyButtonText: {
    color: 'white',
    textAlign: 'center',
    fontWeight: '700',
  },
  cancelText: {
    textAlign: 'center',
    marginTop: 12,
    color: '#434654',
  },
});