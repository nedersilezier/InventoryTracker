import { Ionicons } from "@expo/vector-icons";
//router for navigation
import { useRouter } from "expo-router";
//hooks used for screen state, derived values and initial data loading.
import { useEffect, useMemo, useState } from "react";
import {
  ActivityIndicator,
  Alert,
  Pressable,
  ScrollView,
  StyleSheet,
  Text,
  View,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { CreateTransactionHeader } from "./CreateTransactionHeader";
import { GeneralStep } from "./GeneralStep";
import { ItemsStep } from "./ItemsStep";
import { ReviewStep } from "./ReviewStep";
import { SearchPickerModal } from "./SearchPickerModal";
import {
  createTransaction,
  getClientsLookup,
  getItemsLookup,
  getWarehousesLookup,
} from "../../lib/api";
import {
  ClientLookup,
  CreateTransactionForm,
  ItemLookup,
  OpenPicker,
  PickerOption,
  SelectedTransactionItem,
  TransactionTypeKey,
  WarehouseLookup,
} from "../../lib/create-edit-transaction.types";
import { isIntegerUnit } from "../../lib/create-edit-transaction.utils";

// Local wizard step type
//flow is handled inside one screen using step type instead of routing between separate screens
type Step = "general" | "items" | "review";
type TransactionFormMode = "create" | "edit";
type Props = {
  mode: TransactionFormMode;
  initialForm: CreateTransactionForm;
  headerTitle: string;
  submittingLabel: string;
  submitLabel: string;
  onSubmit: (form: CreateTransactionForm) => Promise<void>;
};

export function TransactionForm({
  mode,
  initialForm,
  headerTitle,
  submittingLabel,
  submitLabel,
  onSubmit,
}: Props) {
  const router = useRouter();

  // Current wizard step
  // general = transaction type, route and metadata | items = item selection and quantity input | review = final confirmation before submit
  const [step, setStep] = useState<Step>("general");
  // Shows initial loading spinner while warehouses, clients and items are loaded
  const [loadingLookups, setLoadingLookups] = useState(true);
  // Prevents duplicate submit requests and disables actions during creation
  const [submitting, setSubmitting] = useState(false);

  // Lookup data loaded from API
  // these arrays are passed to child components and search picker modal
  const [warehouses, setWarehouses] = useState<WarehouseLookup[]>([]);
  const [clients, setClients] = useState<ClientLookup[]>([]);
  const [items, setItems] = useState<ItemLookup[]>([]);
  // Temporary state for the "Add Item" form.
  const [selectedItemId, setSelectedItemId] = useState("");
  const [quantityInput, setQuantityInput] = useState("");
  // Shared search picker modal state
  // one modal is reused for warehouses, clients and items
  const [pickerVisible, setPickerVisible] = useState(false);
  const [pickerTitle, setPickerTitle] = useState("");
  const [pickerSearchPlaceholder, setPickerSearchPlaceholder] = useState("");
  const [pickerOptions, setPickerOptions] = useState<PickerOption[]>([]);
  const [pickerSelectedId, setPickerSelectedId] = useState<
    string | undefined
  >();
  // Selection callback for the currently opened picker
  // each field provides its own onSelect behavior when opening the modal.
  const [pickerOnSelect, setPickerOnSelect] = useState<
    ((option: PickerOption) => void) | null
  >(null);
  // Main form state for the whole wizard
  const [form, setForm] = useState<CreateTransactionForm>(initialForm);
  // Finds the full item object for selectedItemId
  // this is needed for unit validation and display label in the Items step.
  const selectedItem = useMemo(
    () => items.find((item) => item.itemId === selectedItemId),
    [items, selectedItemId],
  );
  // Opens the shared search picker modal with caller-specific options
  // setPickerOnSelect uses a wrapper so React stores the callback itself instead of treating it as a functional state updater
  const openPicker: OpenPicker = ({
    title,
    searchPlaceholder,
    options,
    selectedId,
    onSelect,
  }) => {
    setPickerTitle(title);
    setPickerSearchPlaceholder(searchPlaceholder);
    setPickerOptions(options);
    setPickerSelectedId(selectedId);
    setPickerOnSelect(() => onSelect);
    setPickerVisible(true);
  };

  useEffect(() => {
    async function loadLookups() {
      try {
        setLoadingLookups(true);
        // Load lookup lists in parallel
        const [warehousesResult, clientsResult, itemsResult] =
          await Promise.all([
            getWarehousesLookup(),
            getClientsLookup(),
            getItemsLookup(),
          ]);

        setWarehouses(warehousesResult);
        setClients(clientsResult);
        setItems(itemsResult);
      } catch (error) {
        const message =
          error instanceof Error ? error.message : "Unknown error";
        Alert.alert("Lookup error", message);
      } finally {
        setLoadingLookups(false);
      }
    }

    loadLookups();
  }, []);
  // Updates transaction type and clears dependent fields
  // prevents stale IDs or invalid items from staying after switching transaction type
  const updateType = (type: TransactionTypeKey) => {
    setForm((prev) => ({
      ...prev,
      type,
      fromId: undefined,
      toId: undefined,
      // items are cleared because quantity rules depend on transaction type
      items: [],
    }));

    setSelectedItemId("");
    setQuantityInput("");
  };
  // Validates the first step before allowing the user to continue
  const validateGeneral = () => {
    // Adjustment has no source party, so "From" is not required.
    if (form.type !== "adjustment" && !form.fromId) {
      Alert.alert("Validation", "From field is required.");
      return false;
    }
    // every transaction type needs a target
    if (!form.toId) {
      Alert.alert("Validation", "To field is required.");
      return false;
    }
    // transfersd move stock between two different warehouses
    if (form.type === "transfer" && form.fromId === form.toId) {
      Alert.alert(
        "Validation",
        "Source and destination warehouses must be different.",
      );
      return false;
    }

    return true;
  };
  // Validates the item step before review
  const validateItems = () => {
    if (form.items.length === 0) {
      Alert.alert("Validation", "Add at least one item.");
      return false;
    }

    return true;
  };
  // Adds currently selected item to the transaction
  // if the item already exists, quantities are merged
  const addItem = () => {
    if (!selectedItem) {
      Alert.alert("Validation", "Select an item first.");
      return;
    }

    const quantity = Number(quantityInput.replace(",", "."));

    if (!quantity || quantity === 0) {
      Alert.alert("Validation", "Quantity cannot be 0.");
      return;
    }

    // only adjustment may use negative quantities
    if (form.type !== "adjustment" && quantity < 0) {
      Alert.alert(
        "Validation",
        "Negative quantity is allowed only for adjustment.",
      );
      return;
    }
    // some units(eg pcs) must accept only integers
    if (
      isIntegerUnit(selectedItem.unitOfMeasure) &&
      !Number.isInteger(quantity)
    ) {
      Alert.alert("Validation", "This item requires a whole number quantity.");
      return;
    }

    setForm((prev) => {
      const existing = prev.items.find((x) => x.itemId === selectedItem.itemId);

      if (existing) {
        const nextQuantity = existing.quantity + quantity;
        // Prevent merging from producing a negative quantity for non-adjustment transactions
        if (prev.type !== "adjustment" && nextQuantity < 0) {
          Alert.alert("Validation", "Quantity cannot become negative.");
          return prev;
        }

        return {
          ...prev,
          items: prev.items.map((x) =>
            x.itemId === selectedItem.itemId
              ? { ...x, quantity: nextQuantity }
              : x,
          ),
        };
      }
      // Store display fields in form state so the review screen can show them without resolving item details again
      const newItem: SelectedTransactionItem = {
        itemId: selectedItem.itemId,
        name: selectedItem.name,
        unitOfMeasure: selectedItem.unitOfMeasure,
        quantity,
      };

      return {
        ...prev,
        items: [...prev.items, newItem],
      };
    });
    // Clear temporary add-item inputs after succuess
    setSelectedItemId("");
    setQuantityInput("");
  };

  // Removes an item from the selected transaction items list
  const removeItem = (itemId: string) => {
    setForm((prev) => ({
      ...prev,
      items: prev.items.filter((item) => item.itemId !== itemId),
    }));
  };

  // Final submit handler
  // validates all steps, maps UI form state into API request shape, sends POST request and returns to the transaction list after success
  const submit = async () => {
    if (!validateGeneral() || !validateItems()) return;

    try {
      setSubmitting(true);

      await onSubmit(form);
      // replace prevents user from going back to the filled form and accidentally submitting the same transaction again
      router.replace("/transactions");
    } catch (error) {
      const message = error instanceof Error ? error.message : "Unknown error";
      Alert.alert(mode == "create" ? "Create transaction error" : "Update transaction error", message);
    } finally {
      setSubmitting(false);
    }
  };

  // Moves the wizard forward. Each step validates its own data before the user can continue
  const goNext = () => {
    if (step === "general") {
      if (!validateGeneral()) return;
      setStep("items");
      return;
    }

    if (step === "items") {
      if (!validateItems()) return;
      setStep("review");
    }
  };

  // Handles back behavior inside the wizard
  // Review -> Items -> General -> Transactions list
  const goBack = () => {
    if (step === "review") {
      setStep("items");
      return;
    }

    if (step === "items") {
      setStep("general");
      return;
    }

    router.back();
  };

  // Header title changes based on the current wizard step
  const screenTitle =
    step === "general"
      ? headerTitle
      : step === "items"
        ? "Transaction Items"
        : "Review";
  // While lookup data is loading, show only header and spinner. Prevents rendering picker fields empty
  if (loadingLookups) {
    return (
      <SafeAreaView style={styles.root} edges={["top"]}>
        <CreateTransactionHeader title="New Transaction" onBack={goBack} />

        <View style={styles.center}>
          <ActivityIndicator size="large" />
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={styles.root} edges={["top"]}>
      <CreateTransactionHeader title={screenTitle} onBack={goBack} />

      <ScrollView
        contentContainerStyle={[
          styles.content,
          // Review screen needs extra bottom padding because the sticky bottom bar
          // can otherwise overlap the last item summary row
          step === "review" && styles.reviewContent,
        ]}
      >
        {/* Render only the currently active wizard step.
            Parent screen owns the state; child components only render UI and call callbacks */}
        {step === "general" ? (
          <GeneralStep
            form={form}
            warehouses={warehouses}
            clients={clients}
            onTypeChange={updateType}
            onChange={setForm}
            openPicker={openPicker}
          />
        ) : null}

        {step === "items" ? (
          <ItemsStep
            form={form}
            items={items}
            selectedItemId={selectedItemId}
            quantityInput={quantityInput}
            onSelectedItemChange={setSelectedItemId}
            onQuantityChange={setQuantityInput}
            onAddItem={addItem}
            onRemoveItem={removeItem}
            openPicker={openPicker}
          />
        ) : null}

        {step === "review" ? (
          <ReviewStep form={form} warehouses={warehouses} clients={clients} />
        ) : null}
      </ScrollView>
      {/* Shared fullscreen picker used by route fields and item selection
          receives different options depending on which field opened it */}
      <SearchPickerModal
        visible={pickerVisible}
        title={pickerTitle}
        searchPlaceholder={pickerSearchPlaceholder}
        options={pickerOptions}
        selectedId={pickerSelectedId}
        onClose={() => setPickerVisible(false)}
        onSelect={(option) => {
          pickerOnSelect?.(option);
          setPickerVisible(false);
        }}
      />
      {/* Sticky bottom action bar.
          On Review step it shows Back + Create.
          On first two steps it shows one action - continue */}
      <View style={styles.bottomBar}>
        {step === "review" ? (
          <>
            <Pressable
              style={[
                styles.secondaryButton,
                submitting && styles.disabledButton,
              ]}
              onPress={goBack}
              disabled={submitting}
            >
              <Text style={styles.secondaryButtonText}>Back</Text>
            </Pressable>

            <Pressable
              style={[
                styles.primaryButton,
                submitting && styles.disabledButton,
              ]}
              onPress={submit}
              disabled={submitting}
            >
              <Text style={styles.primaryButtonText}>
                {submitting ? submittingLabel : submitLabel}
              </Text>
            </Pressable>
          </>
        ) : (
          <Pressable style={styles.primaryButton} onPress={goNext}>
            <Text style={styles.primaryButtonText}>
              {step === "general" ? "Next: Items" : "Review Transaction"}
            </Text>
            <Ionicons name="arrow-forward" size={20} color="#ffffff" />
          </Pressable>
        )}
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  root: {
    flex: 1,
    backgroundColor: "#f9f9ff",
  },
  content: {
    padding: 16,
    paddingBottom: 112,
  },
  center: {
    flex: 1,
    alignItems: "center",
    justifyContent: "center",
  },
  // Absolute bottom bar keeps the primary action visible at all times
  // ScrollView content needs enough bottom padding so content is not hidden behind it
  bottomBar: {
    position: "absolute",
    left: 0,
    right: 0,
    bottom: 0,
    padding: 16,
    paddingBottom: 20,
    backgroundColor: "#ffffff",
    borderTopWidth: 1,
    borderTopColor: "#dfe1e6",
    gap: 10,
  },
  primaryButton: {
    minHeight: 54,
    borderRadius: 14,
    backgroundColor: "#0052cc",
    alignItems: "center",
    justifyContent: "center",
    flexDirection: "row",
    gap: 8,
    paddingHorizontal: 16,
  },
  primaryButtonText: {
    color: "#ffffff",
    fontSize: 17,
    fontWeight: "800",
  },
  secondaryButton: {
    minHeight: 52,
    borderRadius: 14,
    borderWidth: 1,
    borderColor: "#737685",
    alignItems: "center",
    justifyContent: "center",
  },
  secondaryButtonText: {
    color: "#0052cc",
    fontSize: 16,
    fontWeight: "700",
  },
  disabledButton: {
    opacity: 0.6,
  },
  reviewContent: {
    paddingBottom: 180,
  },
});
