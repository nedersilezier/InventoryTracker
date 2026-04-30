import { StyleSheet, Text, View } from 'react-native';
import {
  ClientLookup,
  CreateTransactionForm,
  TransactionTypeKey,
  WarehouseLookup,
} from '../../lib/create-transaction.types';
import { formatDisplayDate, getTypeLabel } from '../../lib/create-transaction.utils';
import { SectionTitle } from './SectionTitle';

// Props for the final review step.
// This component only displays already collected form data
type Props = {
  form: CreateTransactionForm;
  warehouses: WarehouseLookup[];
  clients: ClientLookup[];
};

export function ReviewStep({ form, warehouses, clients }: Props) {
  // Convert selected fromId/toId into readable names for display
  // the meaning of fromId/toId depends on transaction type
  const fromName = resolvePartyName(form.type, 'from', form.fromId, warehouses, clients);
  const toName = resolvePartyName(form.type, 'to', form.toId, warehouses, clients);

  return (
    <>
      <Text style={styles.reviewTitle}>Verify Details</Text>
      <Text style={styles.reviewSubtitle}>
        Please review the transaction before final submission.
      </Text>

      <View style={styles.card}>
        <View style={styles.reviewTopRow}>
          <View style={styles.reviewBadge}>
            <Text style={styles.reviewBadgeText}>{getTypeLabel(form.type)}</Text>
          </View>

          {form.referenceNumber ? (
            <Text style={styles.reviewReference}>REF: {form.referenceNumber}</Text>
          ) : null}
        </View>

        <ReviewRow label="Date" value={formatDisplayDate(new Date())} />

        {form.type !== 'adjustment' ? (
          <ReviewRow label="From" value={fromName || '-'} />
        ) : null}

        <ReviewRow
          label={form.type === 'adjustment' ? 'Warehouse' : 'To'}
          value={toName || '-'}
        />

        {form.notes ? <ReviewRow label="Notes" value={form.notes} /> : null}
      </View>

      <SectionTitle title={`Items Summary (${form.items.length})`} />

      <View style={styles.card}>
        {form.items.map((item) => (
          <View key={item.itemId} style={styles.reviewItemRow}>
            <View>
              <Text style={styles.itemName}>{item.name}</Text>
              <Text style={styles.itemMeta}>Unit: {String(item.unitOfMeasure)}</Text>
            </View>

            <Text style={styles.reviewQuantity}>{item.quantity}</Text>
          </View>
        ))}
      </View>
    </>
  );
}

// Resolves mobile fromId/toId values into display names
function resolvePartyName(
  type: TransactionTypeKey,
  side: 'from' | 'to',
  id: string | undefined,
  warehouses: WarehouseLookup[],
  clients: ClientLookup[]
) {
  if (!id) return '';

  if (type === 'issue') {
    return side === 'from'
      ? warehouses.find((x) => x.warehouseId === id)?.name
      : clients.find((x) => x.clientId === id)?.name;
  }

  if (type === 'return') {
    return side === 'from'
      ? clients.find((x) => x.clientId === id)?.name
      : warehouses.find((x) => x.warehouseId === id)?.name;
  }

  return warehouses.find((x) => x.warehouseId === id)?.name;
}

// Small reusable row used inside the review summary card
function ReviewRow({ label, value }: { label: string; value: string }) {
  return (
    <View style={styles.reviewRow}>
      <Text style={styles.reviewLabel}>{label}</Text>
      <Text style={styles.reviewValue}>{value}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  reviewTitle: {
    fontSize: 28,
    fontWeight: '800',
    color: '#041b3c',
    marginTop: 12,
  },
  reviewSubtitle: {
    fontSize: 15,
    color: '#5c5f60',
    marginTop: 6,
    marginBottom: 12,
  },
  card: {
    backgroundColor: '#ffffff',
    borderWidth: 1,
    borderColor: '#c3c6d6',
    borderRadius: 16,
    padding: 16,
    gap: 14,
  },
  reviewTopRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  reviewBadge: {
    backgroundColor: '#0052cc',
    borderRadius: 999,
    paddingHorizontal: 12,
    paddingVertical: 6,
  },
  reviewBadgeText: {
    color: '#ffffff',
    fontWeight: '800',
  },
  reviewReference: {
    fontSize: 13,
    color: '#737685',
    fontWeight: '700',
  },
  reviewRow: {
    borderTopWidth: 1,
    borderTopColor: '#e5e7eb',
    paddingTop: 12,
  },
  reviewLabel: {
    fontSize: 12,
    color: '#737685',
    fontWeight: '700',
    textTransform: 'uppercase',
  },
  reviewValue: {
    marginTop: 4,
    fontSize: 16,
    color: '#041b3c',
    fontWeight: '600',
  },
  reviewItemRow: {
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#e5e7eb',
    flexDirection: 'row',
    justifyContent: 'space-between',
    gap: 12,
  },
  itemName: {
    fontSize: 16,
    fontWeight: '700',
    color: '#041b3c',
  },
  itemMeta: {
    marginTop: 2,
    fontSize: 14,
    color: '#5c5f60',
  },
  reviewQuantity: {
    fontSize: 22,
    fontWeight: '800',
    color: '#0052cc',
  },
});