import { useRouter } from "expo-router";
import { TransactionForm } from "../components/create-transaction/TransactionForm";
import { createTransaction } from "../lib/api";
import { CreateTransactionForm } from "../lib/create-edit-transaction.types";
import { mapFormToCreateRequest } from "../lib/create-edit-transaction.utils";

const EMPTY_TRANSACTION_FORM: CreateTransactionForm = {
  type: "adjustment",
  referenceNumber: "",
  notes: "",
  fromId: undefined,
  toId: undefined,
  items: [],
};

export default function CreateTransactionScreen() {
  const router = useRouter();

  return (
    <TransactionForm
      mode="create"
      initialForm={EMPTY_TRANSACTION_FORM}
      headerTitle="New Transaction"
      submitLabel="Create Transaction"
      submittingLabel="Creating..."
      onSubmit={async (form) => {
        const payload = mapFormToCreateRequest(form);

        await createTransaction(payload);

        router.replace("/transactions");
      }}
      onCancel={async () => {}}
    />
  );
}
