import { View, Pressable, StyleSheet, Text } from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { useRouter } from 'expo-router';

type Props = {
  onOpenFilters: () => void;
};

export function TransactionsHeader({onOpenFilters} : Props)
{
  const router = useRouter();
    return(
    <View style={styles.header}>
        <Pressable style={styles.iconButton}>
          <Ionicons name="menu" size={28} color="#041b3c" />
        </Pressable>

        <Text style={styles.headerTitle}>Transactions</Text>

        <View style={styles.headerActions}>
          <Pressable style={styles.iconButton} 
                    onPress={ onOpenFilters }>
            <Ionicons name="filter" size={24} color="#041b3c" />
          </Pressable>

          <Pressable style={styles.iconButton}
                      onPress={() => router.push('/create-transaction')}>
            <Ionicons name="add" size={30} color="#0052cc" />
          </Pressable>
        </View>
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
    flex: 1,
    fontSize: 22,
    fontWeight: '700',
    color: '#041b3c',
    marginLeft: 8,
  },
  headerActions: {
    flexDirection: 'row',
    alignItems: 'center',
  },
});