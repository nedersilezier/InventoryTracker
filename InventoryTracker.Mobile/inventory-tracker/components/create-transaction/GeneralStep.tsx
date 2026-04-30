import { StyleSheet, Text, View } from "react-native";
import {
  ClientLookup,
  CreateTransactionForm,
  OpenPicker,
  PickerOption,
  TransactionTypeKey,
  WarehouseLookup,
} from "../../lib/create-transaction.types";
import {
  formatDisplayDate,
  getRouteLabels,
} from "../../lib/create-transaction.utils";
import { ReadOnlyInfoField } from "./ReadOnlyInfoField";
import { SearchPickerField } from "./SearchPickerField";
import { SectionTitle } from "./SectionTitle";
import { TextField } from "./TextField";
import { TransactionTypeSelector } from "./TransactionTypeSelector";

// Props for the first step of the create transaction wizard.
// This step owns only the general transaction data and party selection.
// Actual state is kept in the parent screen
type Props = {
  form: CreateTransactionForm;
  warehouses: WarehouseLookup[];
  clients: ClientLookup[];
  onTypeChange: (type: TransactionTypeKey) => void;
  onChange: (form: CreateTransactionForm) => void;
  openPicker: OpenPicker;
};

export function GeneralStep({
  form,
  warehouses,
  clients,
  onTypeChange,
  onChange,
  openPicker,
}: Props) {
  // Defines labels and visibility rules for From/To fields based on transaction type.
  const routeLabels = getRouteLabels(form.type);

  // Convert warehouse/client lookup data into generic picker options.
  // The search picker works with { id, label }, regardless of entity type.
  const warehouseOptions: PickerOption[] = warehouses.map((x) => ({
    id: x.warehouseId,
    label: x.name,
  }));

  const clientOptions: PickerOption[] = clients.map((x) => ({
    id: x.clientId,
    label: x.name,
  }));
  // Decide what kind of entity the "From" field should select
  const fromOptions = form.type === "return" ? clientOptions : warehouseOptions;
  // Decide what kind of entity the "To" field should select
  const toOptions = form.type === "issue" ? clientOptions : warehouseOptions;

  // Resolve selected IDs to labels so fields can display readable names.
  const fromValueLabel = getSelectedLabel(fromOptions, form.fromId);
  const toValueLabel = getSelectedLabel(toOptions, form.toId);

  return (
    <>
      <TransactionTypeSelector value={form.type} onChange={onTypeChange} />

      <SectionTitle title="General Information" />

      <ReadOnlyInfoField
        label="Transaction Date"
        value={formatDisplayDate(new Date())}
        helperText="Date is assigned automatically for real-time mobile transactions."
      />

      <TextField
        label="Reference Number"
        value={form.referenceNumber}
        placeholder="Optional"
        onChangeText={(v) => onChange({ ...form, referenceNumber: v })}
      />

      <TextField
        label="Notes"
        value={form.notes}
        placeholder="Optional notes"
        multiline
        onChangeText={(v) => onChange({ ...form, notes: v })}
      />

      <SectionTitle title="Route / Parties" />

      <View style={styles.card}>
        {routeLabels.showFrom ? (
          <SearchPickerField
            label={routeLabels.fromLabel}
            valueLabel={fromValueLabel}
            placeholder={routeLabels.fromLabel}
            onPress={() =>
              // Open the shared search picker with options appropriate for "From" field
              openPicker({
                title: routeLabels.fromLabel,
                searchPlaceholder: `Search ${routeLabels.fromLabel.toLowerCase()}...`,
                options: fromOptions,
                selectedId: form.fromId,
                onSelect: (option) => onChange({ ...form, fromId: option.id }),
              })
            }
          />
        ) : (
          <Text style={styles.helperText}>
            Adjustment affects a single warehouse. Negative quantities are
            allowed.
          </Text>
        )}

        <SearchPickerField
          label={routeLabels.toLabel}
          valueLabel={toValueLabel}
          placeholder={routeLabels.toLabel}
          onPress={() =>
            // Open the shared search picker for the "To" field
            openPicker({
              title: routeLabels.toLabel,
              searchPlaceholder: `Search ${routeLabels.toLabel.toLowerCase()}...`,
              options: toOptions,
              selectedId: form.toId,
              onSelect: (option) => onChange({ ...form, toId: option.id }),
            })
          }
        />

        {form.type === "transfer" ? (
          <Text style={styles.helperText}>
            Source and destination warehouses must be different.
          </Text>
        ) : null}
      </View>
    </>
  );
}

// Converts selected ID into a display label for picker fields. Empty if nothing selected
function getSelectedLabel(options: PickerOption[], id?: string) {
  if (!id) return "";

  return options.find((option) => option.id === id)?.label ?? "";
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
});
