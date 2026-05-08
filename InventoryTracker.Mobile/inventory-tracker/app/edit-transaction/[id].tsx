import { useLocalSearchParams, useRouter } from "expo-router";
import { useEffect, useState } from "react";
import { ActivityIndicator, Alert, View } from "react-native";
import { TransactionForm } from "../../components/create-transaction/TransactionForm";
import {
  cancelTransaction,
  getTransactionById,
  updateTransaction,
} from "../../lib/api";

import {
  CancelTransactionRequest,
  CreateTransactionForm,
} from "../../lib/create-edit-transaction.types";
import {
  mapFormToCreateRequest,
  mapTransactionForEditToForm,
} from "../../lib/create-edit-transaction.utils";

export default function EditTransactionScreen() {
  const router = useRouter();
  const { id } = useLocalSearchParams<{ id: string }>();

  const [initialForm, setInitialForm] = useState<CreateTransactionForm | null>(
    null,
  );

  useEffect(() => {
    async function loadTransaction() {
      try {
        if (!id) return;

        const transaction = await getTransactionById(id);

        setInitialForm(mapTransactionForEditToForm(transaction));
      } catch (error) {
        const message =
          error instanceof Error ? error.message : "Unknown error";

        Alert.alert("Load transaction error", message);
      }
    }

    loadTransaction();
  }, [id]);

  if (!initialForm) {
    return (
      <View style={{ flex: 1, justifyContent: "center" }}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <TransactionForm
      mode="edit"
      initialForm={initialForm}
      headerTitle="Edit Transaction"
      submitLabel="Save Changes"
      submittingLabel="Saving..."
      onSubmit={async (form) => {
        if (!id) return;

        const payload = mapFormToCreateRequest(form);

        await updateTransaction(id, payload);

        router.replace("/transactions");
      }}
      onCancel={async () => {
        if (!id) return;
        const payload: CancelTransactionRequest = {
          cancellationReason:
            "Cancelled from mobile. Cancellation on mobile to be changed.",
        };
        await cancelTransaction(id, payload);
        router.replace("/transactions");
      }}
    />
  );
}
