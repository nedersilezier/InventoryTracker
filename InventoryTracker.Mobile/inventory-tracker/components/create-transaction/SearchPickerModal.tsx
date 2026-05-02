import { Ionicons } from "@expo/vector-icons";
import { useMemo, useState } from "react";
import {
  FlatList,
  Modal,
  Pressable,
  StyleSheet,
  Text,
  TextInput,
  View,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { PickerOption } from "../../lib/create-edit-transaction.types";

// Props for the reusable fullscreen search picker
// same modal is used for warehouses, clients and items
type Props = {
  visible: boolean;
  title: string;
  searchPlaceholder: string;
  options: PickerOption[];
  selectedId?: string;
  onSelect: (option: PickerOption) => void;
  onClose: () => void;
};

export function SearchPickerModal({
  visible,
  title,
  searchPlaceholder,
  options,
  selectedId,
  onSelect,
  onClose,
}: Props) {
  // Local search input state
  const [searchTerm, setSearchTerm] = useState("");
  // Filter options locally based on label and optional subtitle
  // useMemo avoids recalculating the filtered list unless options or search term changes
  const filteredOptions = useMemo(() => {
    const normalized = searchTerm.trim().toLowerCase();

    if (!normalized) return options;

    return options.filter((option) => {
      const label = option.label.toLowerCase();
      const subtitle = option.subtitle?.toLowerCase() ?? "";

      return label.includes(normalized) || subtitle.includes(normalized);
    });
  }, [options, searchTerm]);

  return (
    <Modal
      visible={visible}
      animationType="slide"
      // Fullscreen modal is used instead of a native select/dropdown
      // because long mobile lists need more space and search input
      presentationStyle="fullScreen"
      onRequestClose={onClose}
    >
      <SafeAreaView style={styles.pickerRoot} edges={["top"]}>
        <View style={styles.pickerHeader}>
          <Pressable style={styles.iconButton} onPress={onClose}>
            <Ionicons name="close" size={26} color="#041b3c" />
          </Pressable>

          <Text style={styles.pickerTitle}>{title}</Text>
        </View>

        <View style={styles.searchBox}>
          <Ionicons name="search-outline" size={20} color="#737685" />

          <TextInput
            value={searchTerm}
            onChangeText={setSearchTerm}
            placeholder={searchPlaceholder}
            // Open keyboard immediately
            autoFocus
            style={styles.searchInput}
          />

          {searchTerm ? (
            // clear button for the current search query
            <Pressable onPress={() => setSearchTerm("")}>
              <Ionicons name="close-circle" size={20} color="#737685" />
            </Pressable>
          ) : null}
        </View>

        <FlatList
          data={filteredOptions}
          keyExtractor={(item) => item.id}
          // Allows tapping list items while the keyboard is open
          keyboardShouldPersistTaps="handled"
          contentContainerStyle={styles.pickerListContent}
          ListEmptyComponent={
            <View style={styles.emptyCard}>
              <Text style={styles.emptyTitle}>No results</Text>
              <Text style={styles.emptyText}>Try a different search term.</Text>
            </View>
          }
          renderItem={({ item }) => {
            // Used to highlight the currently selected option
            const active = item.id === selectedId;

            return (
              <Pressable
                style={[
                  styles.pickerOption,
                  active && styles.pickerOptionActive,
                ]}
                onPress={() => onSelect(item)}
              >
                <View style={{ flex: 1 }}>
                  <Text
                    style={[
                      styles.pickerOptionLabel,
                      active && styles.pickerOptionLabelActive,
                    ]}
                  >
                    {item.label}
                  </Text>

                  {item.subtitle ? (
                    <Text style={styles.pickerOptionSubtitle}>
                      {item.subtitle}
                    </Text>
                  ) : null}
                </View>

                {active ? (
                  <Ionicons name="checkmark-circle" size={22} color="#0052cc" />
                ) : null}
              </Pressable>
            );
          }}
        />
      </SafeAreaView>
    </Modal>
  );
}

const styles = StyleSheet.create({
  pickerRoot: {
    flex: 1,
    backgroundColor: "#f9f9ff",
  },
  pickerHeader: {
    height: 56,
    paddingHorizontal: 16,
    backgroundColor: "#ffffff",
    borderBottomWidth: 1,
    borderBottomColor: "#dfe1e6",
    flexDirection: "row",
    alignItems: "center",
  },
  iconButton: {
    width: 44,
    height: 44,
    alignItems: "center",
    justifyContent: "center",
  },
  pickerTitle: {
    fontSize: 20,
    fontWeight: "800",
    color: "#041b3c",
    marginLeft: 8,
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
  pickerListContent: {
    paddingHorizontal: 16,
    paddingBottom: 24,
  },
  pickerOption: {
    minHeight: 64,
    borderRadius: 14,
    borderWidth: 1,
    borderColor: "#dfe1e6",
    backgroundColor: "#ffffff",
    paddingHorizontal: 14,
    paddingVertical: 12,
    marginBottom: 10,
    flexDirection: "row",
    alignItems: "center",
    gap: 12,
  },
  pickerOptionActive: {
    borderColor: "#0052cc",
    backgroundColor: "#e8edff",
  },
  pickerOptionLabel: {
    fontSize: 16,
    fontWeight: "700",
    color: "#041b3c",
  },
  pickerOptionLabelActive: {
    color: "#0052cc",
  },
  pickerOptionSubtitle: {
    marginTop: 2,
    fontSize: 13,
    color: "#5c5f60",
  },
  emptyCard: {
    borderRadius: 16,
    borderWidth: 1,
    borderColor: "#dfe1e6",
    backgroundColor: "#ffffff",
    padding: 18,
    alignItems: "center",
  },
  emptyTitle: {
    fontSize: 16,
    fontWeight: "700",
    color: "#041b3c",
  },
  emptyText: {
    marginTop: 4,
    fontSize: 14,
    color: "#5c5f60",
  },
});
