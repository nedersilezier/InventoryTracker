import { Pressable, StyleSheet, Text, View, TextInput } from "react-native";
import { TransactionFilters } from "../../lib/transactions.types";
import { Ionicons } from "@expo/vector-icons";

type Props = {
  visible: boolean;
  filters: TransactionFilters;
  onChange: (filters: TransactionFilters) => void;
  onApply: () => void;
  onClose: () => void;
};

export function SearchModal({
  visible,
  filters,
  onChange,
  onApply,
  onClose,
}: Props) {
  if (!visible) return null;

  return (
    <View style={styles.overlay}>
      <View style={styles.modal}>
        <Text style={styles.title}>Search</Text>

        <View style={styles.searchBox}>
          <Ionicons name="search-outline" size={20} color="#737685" />

          <TextInput
            value={filters.searchTerm ?? ""}
            onChangeText={(text) => {
              onChange({ ...filters, searchTerm: text });
            }}
            placeholder="Warehouse, Client, Reference..."
            // Open keyboard immediately
            autoFocus
            style={styles.searchInput}
          />

          {filters.searchTerm ? (
            // clear button for the current search query
            <Pressable
              onPress={() => {
                onChange({ ...filters, searchTerm: "" });
              }}
            >
              <Ionicons name="close-circle" size={20} color="#737685" />
            </Pressable>
          ) : null}
        </View>

        <View style={{ height: 16 }} />

        <Pressable style={styles.applyButton} onPress={onApply}>
          <Text style={styles.applyButtonText}>Apply search</Text>
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
    position: "absolute",
    top: 0,
    left: 0,
    right: 0,
    bottom: 70,
    backgroundColor: "rgba(0,0,0,0.4)",
    justifyContent: "flex-end",
  },
  modal: {
    backgroundColor: "white",
    padding: 20,
    borderTopLeftRadius: 16,
    borderTopRightRadius: 16,
  },
  title: {
    fontSize: 18,
    fontWeight: "700",
    marginBottom: 16,
    color: "#041b3c",
  },
  applyButton: {
    backgroundColor: "#0052cc",
    padding: 14,
    borderRadius: 12,
  },
  applyButtonText: {
    color: "white",
    textAlign: "center",
    fontWeight: "700",
  },
  cancelText: {
    textAlign: "center",
    marginTop: 12,
    color: "#434654",
  },
  searchBox: {
    margin: 16,
    height: 52,
    borderRadius: 14,
    borderWidth: 1,
    borderColor: "#c3c6d6",
    backgroundColor: "#ffffff",
    paddingHorizontal: 14,
    flexDirection: "row",
    alignItems: "center",
    gap: 10,
  },
  searchInput: {
    flex: 1,
    fontSize: 16,
    color: "#041b3c",
  },
});
