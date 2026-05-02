import { Ionicons } from "@expo/vector-icons";
import { Pressable, StyleSheet, Text, View } from "react-native";
import {
  CreateTransactionForm,
  ItemLookup,
  OpenPicker,
  PickerOption,
} from "../../lib/create-edit-transaction.types";
import { isIntegerUnit } from "../../lib/create-edit-transaction.utils";
import { SearchPickerField } from "./SearchPickerField";
import { SectionTitle } from "./SectionTitle";
import { TextField } from "./TextField";

// Props for the second step of the create transaction wizard.
// this component renders item selection UI, but the actual state and validation are owned by the parent screen
type Props = {
  form: CreateTransactionForm;
  items: ItemLookup[];
  selectedItemId: string;
  // converted to number in the parent addItem handler.
  quantityInput: string;
  onSelectedItemChange: (id: string) => void;
  onQuantityChange: (value: string) => void;
  onAddItem: () => void;
  onRemoveItem: (id: string) => void;
  // search picker used for item lookup selection.
  openPicker: OpenPicker;
};

export function ItemsStep({
  form,
  items,
  selectedItemId,
  quantityInput,
  onSelectedItemChange,
  onQuantityChange,
  onAddItem,
  onRemoveItem,
  openPicker,
}: Props) {
  // Convert item lookup data into generic picker options.
  // subtitle is used to show unit of measure in the picker list.
  const itemOptions: PickerOption[] = items.map((x) => ({
    id: x.itemId,
    label: x.name,
    subtitle: `Unit: ${String(x.unitOfMeasure)}`,
  }));
  // Find the full item object for the currently selected item ID.
  // needed to display the selected item label and unit-specific helper text.
  const selectedItem = items.find((item) => item.itemId === selectedItemId);
  // Display label shown in the picker field after item selection.
  const selectedItemLabel = selectedItem
    ? `${selectedItem.name} (${String(selectedItem.unitOfMeasure)})`
    : "";

  return (
    <>
      <SectionTitle title="Add Item" />

      <View style={styles.card}>
        <SearchPickerField
          label="Item"
          valueLabel={selectedItemLabel}
          placeholder="Select item"
          onPress={() =>
            // Open shared picker with item options.
            openPicker({
              title: "Select item",
              searchPlaceholder: "Search item...",
              options: itemOptions,
              selectedId: selectedItemId,
              onSelect: (option) => onSelectedItemChange(option.id),
            })
          }
        />

        <TextField
          label="Quantity"
          value={quantityInput}
          //placeholder hints that negative amounts are supported for adjustments
          placeholder={form.type === "adjustment" ? "e.g. -5" : "0"}
          keyboardType="numeric"
          onChangeText={onQuantityChange}
        />

        <Text style={styles.helperText}>
          {selectedItem && isIntegerUnit(selectedItem.unitOfMeasure)
            ? "Integer only for this item."
            : "Decimal values are allowed for this item."}
        </Text>

        <Pressable style={styles.inlinePrimaryButton} onPress={onAddItem}>
          <Ionicons name="add-circle-outline" size={20} color="#ffffff" />
          <Text style={styles.inlinePrimaryButtonText}>Add Item</Text>
        </Pressable>
      </View>

      <SectionTitle title={`Selected Items (${form.items.length})`} />

      {form.items.length === 0 ? (
        // Empty state shown before the user adds any item.
        <View style={styles.emptyCard}>
          <Text style={styles.emptyTitle}>No items added yet</Text>
          <Text style={styles.emptyText}>
            Add at least one item to continue.
          </Text>
        </View>
      ) : (
        form.items.map((item) => (
          // Render selected transaction items.
          // Items are stored in form state because they are part of the final API request.
          <View key={item.itemId} style={styles.itemCard}>
            <View style={styles.itemIcon}>
              <Ionicons name="cube-outline" size={22} color="#0052cc" />
            </View>

            <View style={styles.itemContent}>
              <Text style={styles.itemName}>{item.name}</Text>
              <Text style={styles.itemMeta}>
                Qty: {item.quantity} • Unit: {String(item.unitOfMeasure)}
              </Text>
            </View>

            <Pressable
              style={styles.deleteButton}
              onPress={() => onRemoveItem(item.itemId)}
            >
              <Ionicons name="trash-outline" size={22} color="#ba1a1a" />
            </Pressable>
          </View>
        ))
      )}
    </>
  );
}

const styles = StyleSheet.create({
  card: {
    backgroundColor: "#ffffff",
    borderWidth: 1,
    borderColor: "#c3c6d6",
    borderRadius: 16,
    padding: 16,
    gap: 14,
  },
  helperText: {
    fontSize: 13,
    color: "#5c5f60",
    lineHeight: 18,
  },
  inlinePrimaryButton: {
    height: 50,
    borderRadius: 12,
    backgroundColor: "#0052cc",
    alignItems: "center",
    justifyContent: "center",
    flexDirection: "row",
    gap: 8,
  },
  inlinePrimaryButtonText: {
    color: "#ffffff",
    fontSize: 16,
    fontWeight: "700",
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
  itemCard: {
    flexDirection: "row",
    alignItems: "center",
    backgroundColor: "#ffffff",
    borderRadius: 16,
    borderWidth: 1,
    borderColor: "#dfe1e6",
    padding: 14,
    marginBottom: 10,
    gap: 12,
  },
  itemIcon: {
    width: 44,
    height: 44,
    borderRadius: 12,
    backgroundColor: "#e8edff",
    alignItems: "center",
    justifyContent: "center",
  },
  itemContent: {
    flex: 1,
  },
  itemName: {
    fontSize: 16,
    fontWeight: "700",
    color: "#041b3c",
  },
  itemMeta: {
    marginTop: 2,
    fontSize: 14,
    color: "#5c5f60",
  },
  deleteButton: {
    width: 44,
    height: 44,
    alignItems: "center",
    justifyContent: "center",
  },
});
