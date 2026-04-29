import { Ionicons } from '@expo/vector-icons';
import { Pressable, StyleSheet, Text, View } from 'react-native';
import { TransactionListDTO } from '../../lib/transactions.types';
import { TYPE_UI } from '../../lib/transactions.ui';
import { shortId } from '../../lib/transactions.utils';

type Props = {
  transaction: TransactionListDTO;
};

export function TransactionCard({ transaction }: Props) {
  const type = TYPE_UI[transaction.typeName] ?? {
    label: transaction.typeName || 'Unknown',
    color: '#5c5f60',
    bg: '#dee0e2',
    icon: 'document-text-outline' as keyof typeof Ionicons.glyphMap,
  };

  const title = `${type.label} • ${
    transaction.referenceNumber ?? '-'
  }`;

  return (
    <Pressable style={styles.card}>
      <View style={[styles.typeBar, { backgroundColor: type.color }]} />

      <View style={styles.cardBody}>
        <View style={styles.cardTop}>
          <Text style={styles.cardTitle}>{title}</Text>

          <View style={[styles.badge, { backgroundColor: type.bg }]}>
            <Text style={[styles.badgeText, { color: type.color }]}>
              {type.label}
            </Text>
          </View>
        </View>

        <View style={styles.statusRow}>
          <Ionicons name={type.icon} size={16} color={type.color} />
          <Text style={styles.statusText}>{transaction.statusName}</Text>
        </View>

        <View style={styles.routeBox}>
          {transaction.fromDisplay ? (
            <View style={styles.detailRow}>
              <Ionicons name="arrow-up-outline" size={16} color="#434654" />
              <Text style={styles.detailText}>From: {transaction.fromDisplay}</Text>
            </View>
          ) : null}

          {transaction.toDisplay ? (
            <View style={styles.detailRow}>
              <Ionicons name="arrow-down-outline" size={16} color="#434654" />
              <Text style={styles.detailText}>To: {transaction.toDisplay}</Text>
            </View>
          ) : null}
        </View>
      </View>
    </Pressable>
  );
}

const styles = StyleSheet.create({
  card: {
    flexDirection: 'row',
    backgroundColor: '#f4f5f7',
    borderRadius: 16,
    borderWidth: 1,
    borderColor: '#dfe1e6',
    marginBottom: 14,
    overflow: 'hidden',
    shadowColor: '#091e42',
    shadowOpacity: 0.08,
    shadowRadius: 12,
    shadowOffset: { width: 0, height: 4 },
    elevation: 2,
  },
  typeBar: {
    width: 6,
  },
  cardBody: {
    flex: 1,
    padding: 16,
    gap: 7,
  },
  cardTop: {
    flexDirection: 'row',
    gap: 8,
    alignItems: 'flex-start',
    justifyContent: 'space-between',
  },
  cardTitle: {
    flex: 1,
    fontSize: 17,
    lineHeight: 23,
    fontWeight: '700',
    color: '#041b3c',
  },
  badge: {
    borderRadius: 999,
    paddingHorizontal: 10,
    paddingVertical: 4,
  },
  badgeText: {
    fontSize: 11,
    fontWeight: '800',
    textTransform: 'uppercase',
  },
  statusRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 6,
  },
  statusText: {
    fontSize: 14,
    color: '#5c5f60',
    fontWeight: '600',
  },
  routeBox: {
    gap: 4,
    marginTop: 2,
  },
  detailRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
  detailText: {
    flex: 1,
    fontSize: 14,
    color: '#434654',
  },
});