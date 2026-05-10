import { useRouter } from "expo-router";
import { useCallback, useEffect, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  FlatList,
  RefreshControl,
  StyleSheet,
  Text,
  View,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { BottomNavItem } from "../components/transactions/BottomNavItem";
import { FilterModal } from "../components/transactions/FilterModal";
import { SearchModal } from "../components/transactions/SearchModal";
import { TransactionCard } from "../components/transactions/TransactionCard";
import { TransactionsHeader } from "../components/transactions/TransactionsHeader";
import { getTransactions } from "../lib/api";
import {
  DEFAULT_FILTERS,
  TransactionFilters,
  TransactionListDTO,
} from "../lib/transactions.types";
import { formatDateLabel } from "../lib/transactions.utils";

const PAGE_SIZE = 10;

export default function TransactionsScreen() {
  const router = useRouter();
  //list of transactions in UI
  const [items, setItems] = useState<TransactionListDTO[]>([]);
  //current page number(for pagination)
  const [pageNumber, setPageNumber] = useState(1);
  // total pages number initialized to 1 to avoid edge cases(premature 'end of list' checks)
  const [totalPages, setTotalPages] = useState(1);
  const [initialLoading, setInitialLoading] = useState(true);
  const [loadingMore, setLoadingMore] = useState(false);
  const [refreshing, setRefreshing] = useState(false);
  const [transactionsVisible, setTransactionsVisible] = useState(true);
  const [filterVisible, setFilterVisible] = useState(false);
  const [searchVisible, setSearchVisible] = useState(false);
  //active filters used for api requests
  const [filters, setFilters] = useState<TransactionFilters>(DEFAULT_FILTERS);
  //draft filters used inside filters modal
  const [draftFilters, setDraftFilters] =
    useState<TransactionFilters>(DEFAULT_FILTERS);

  const [applyingFilters, setApplyingFilters] = useState(false);
  const [applyingSearch, setApplyingSearch] = useState(false);

  //core responsible for fetching data
  const loadPage = useCallback(
    async (
      page: number,
      mode: "replace" | "append",
      activeFilters: TransactionFilters,
    ) => {
      console.log(activeFilters);
      const result = await getTransactions({
        pageNumber: page,
        pageSize: PAGE_SIZE,
        ...activeFilters,
      });
      setItems((prev) => {
        //override list(when new filters)
        if (mode === "replace") {
          return result.items;
        }

        //remove duplicated items to avoid pagination related issues
        const existingIds = new Set(prev.map((x) => x.transactionId));
        const newItems = result.items.filter(
          (x) => !existingIds.has(x.transactionId),
        );

        return [...prev, ...newItems];
      });

      setPageNumber(result.pageNumber);
      setTotalPages(result.totalPages);
    },
    [],
  );

  //initialize first page
  useEffect(() => {
    async function init() {
      try {
        setInitialLoading(true);
        await loadPage(1, "replace", DEFAULT_FILTERS);
      } catch (error) {
        const message =
          error instanceof Error ? error.message : "Unknown error";
        Alert.alert("Transactions error", message);
      } finally {
        setInitialLoading(false);
      }
    }

    init();
  }, [loadPage]);

  //reload page with current filters
  const handleRefresh = async () => {
    try {
      setRefreshing(true);
      await loadPage(1, "replace", filters);
    } catch (error) {
      const message = error instanceof Error ? error.message : "Unknown error";
      Alert.alert("Refresh error", message);
    } finally {
      setRefreshing(false);
    }
  };

  //fetch more data when end of page reached
  const handleLoadMore = async () => {
    //stop if already loading or no more pages to load
    if (
      initialLoading ||
      loadingMore ||
      refreshing ||
      applyingFilters ||
      applyingSearch ||
      pageNumber >= totalPages
    ) {
      return;
    }
    //load next page and append results
    try {
      setLoadingMore(true);
      await loadPage(pageNumber + 1, "append", filters);
    } catch (error) {
      const message = error instanceof Error ? error.message : "Unknown error";
      Alert.alert("Pagination error", message);
    } finally {
      setLoadingMore(false);
    }
  };
  //open filters modal with current filters copied to draftFilters helper
  const handleOpenFilters = () => {
    setDraftFilters(filters);
    setFilterVisible(true);
  };
  //open search modal
  const handleOpenSearch = () => {
    setDraftFilters(filters);
    setTransactionsVisible(false);
    setSearchVisible(true);
  };
  //apply filters + reset list and fetch new data
  const handleApplyFilters = async () => {
    const nextFilters = draftFilters;

    try {
      setApplyingFilters(true);
      setFilterVisible(false);
      setFilters(nextFilters);
      setItems([]);
      setPageNumber(1);

      await loadPage(1, "replace", nextFilters);
    } catch (error) {
      const message = error instanceof Error ? error.message : "Unknown error";
      Alert.alert("Filter error", message);
    } finally {
      setApplyingFilters(false);
    }
  };
  //apply search
  const handleApplySearch = async () => {
    const nextFilters = draftFilters;
    console.log(nextFilters);
    try {
      setApplyingSearch(true);
      setSearchVisible(false);
      setTransactionsVisible(true);
      setFilters(nextFilters);
      setItems([]);
      setPageNumber(1);

      await loadPage(1, "replace", nextFilters);
    } catch (error) {
      const message = error instanceof Error ? error.message : "Unknown error";
      Alert.alert("Search error", message);
    } finally {
      setApplyingSearch(false);
    }
  };

  //view
  return (
    <SafeAreaView style={styles.root} edges={["top"]}>
      <TransactionsHeader onOpenFilters={handleOpenFilters} />

      {initialLoading ? (
        <View style={styles.center}>
          <ActivityIndicator size="large" />
        </View>
      ) : (
        //render main list
        <FlatList
          data={items}
          keyExtractor={(item) => item.transactionId}
          contentContainerStyle={styles.listContent}
          onEndReached={handleLoadMore}
          onEndReachedThreshold={0.4}
          refreshControl={
            <RefreshControl refreshing={refreshing} onRefresh={handleRefresh} />
          }
          ListEmptyComponent={
            <View style={styles.empty}>
              <Text style={styles.emptyTitle}>No transactions</Text>
              <Text style={styles.emptyText}>Nothing to display yet.</Text>
            </View>
          }
          ListFooterComponent={
            loadingMore ? (
              <View style={styles.footerLoader}>
                <ActivityIndicator />
              </View>
            ) : null
          }
          renderItem={({ item, index }) => {
            const previous = items[index - 1];
            //show date for the first item and when it changes between items
            const showDate =
              !previous ||
              formatDateLabel(previous.transactionDate) !==
                formatDateLabel(item.transactionDate);

            return (
              <View>
                {showDate && (
                  <Text style={styles.dateLabel}>
                    {formatDateLabel(item.transactionDate)}
                  </Text>
                )}

                <TransactionCard transaction={item} />
              </View>
            );
          }}
        />
      )}
      {/* filters modal view   */}
      <FilterModal
        visible={filterVisible}
        filters={draftFilters}
        onChange={setDraftFilters}
        onApply={handleApplyFilters}
        onClose={() => setFilterVisible(false)}
      />
      {/* search modal view */}
      <SearchModal
        visible={searchVisible}
        filters={draftFilters}
        onChange={setDraftFilters}
        onApply={handleApplySearch}
        onClose={() => {
          setSearchVisible(false);
          setTransactionsVisible(true);
        }}
      />
      {/* bottom navigation view */}
      <View style={styles.bottomNav}>
        <BottomNavItem
          icon="search-outline"
          label="Search"
          active={searchVisible}
          onOpen={handleOpenSearch}
        />
        <BottomNavItem
          icon="receipt-outline"
          label="Transactions"
          active={transactionsVisible}
          onOpen={() => router.replace("/transactions")}
        />
        <BottomNavItem
          icon="person-outline"
          label="Account"
          active={false}
          onOpen={() => {}}
        />
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  root: {
    flex: 1,
    backgroundColor: "#f9f9ff",
  },
  center: {
    flex: 1,
    alignItems: "center",
    justifyContent: "center",
    paddingBottom: 72,
  },
  listContent: {
    padding: 16,
    paddingBottom: 96,
  },
  dateLabel: {
    marginTop: 8,
    marginBottom: 10,
    fontSize: 13,
    fontWeight: "700",
    textTransform: "uppercase",
    letterSpacing: 1.2,
    color: "#434654",
  },
  footerLoader: {
    paddingVertical: 20,
  },
  empty: {
    paddingTop: 80,
    alignItems: "center",
  },
  emptyTitle: {
    fontSize: 18,
    fontWeight: "700",
    color: "#041b3c",
  },
  emptyText: {
    marginTop: 6,
    fontSize: 14,
    color: "#5c5f60",
  },
  bottomNav: {
    position: "absolute",
    bottom: 0,
    left: 0,
    right: 0,
    height: 72,
    paddingHorizontal: 24,
    paddingBottom: 8,
    borderTopWidth: 1,
    borderTopColor: "#dfe1e6",
    backgroundColor: "#ffffff",
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
});
