// Numeric enum transaction type values expected by the api
export enum TransactionTypeValue {
  IssueToClient = 1,
  ReturnFromClient = 2,
  TransferBetweenWarehouses = 3,
  Adjustment = 4,
}

export type TransactionTypeKey = "issue" | "return" | "transfer" | "adjustment";

// Warehouse option returned by lookup endpoint.
export type WarehouseLookup = {
  warehouseId: string;
  name: string;
};

// Client option returned by lookup endpoint.
export type ClientLookup = {
  clientId: string;
  name: string;
};

// Item option returned by lookup endpoint.
export type ItemLookup = {
  itemId: string;
  name: string;
  unitOfMeasure: string;
};

//Item selected by the user inside the create transaction flow
//includes display fields for ui(name, unit of measure)
export type SelectedTransactionItem = {
  itemId: string;
  name: string;
  unitOfMeasure: string;
  quantity: number;
};

// Internal form state used by the create transaction wizard.
// fromId/toId are UI fields and are later mapped to backend-specific IDs depending on transaction type.
export type CreateTransactionForm = {
  type: TransactionTypeKey;
  referenceNumber: string;
  notes: string;
  fromId?: string;
  toId?: string;
  items: SelectedTransactionItem[];
};

// Request body sent to POST /api/user/transactions.
// This shape matches the backend API contract.
export type CreateTransactionRequest = {
  type: TransactionTypeValue;
  clientId?: string | null;
  sourceWarehouseId?: string | null;
  destinationWarehouseId?: string | null;
  transactionDate: string;
  referenceNumber?: string | null;
  notes?: string | null;
  items: {
    itemId: string;
    quantity: number;
  }[];
};

export type CancelTransactionRequest = {
  cancellationReason: string;
};

// Option used by the reusable search picker modal.
// The same picker is used for clients, warehouses and items.
export type PickerOption = {
  id: string;
  label: string;
  subtitle?: string;
};

// Function signature for opening the shared search picker.
// Each field provides its own title, options and selection callback.
export type OpenPicker = (config: {
  title: string;
  searchPlaceholder: string;
  options: PickerOption[];
  selectedId?: string;
  onSelect: (option: PickerOption) => void;
}) => void;

//////
///edit related
/////

export type TransactionForEditDTO = {
  transactionId: string;
  type: TransactionTypeValue;
  status: number;
  clientId: string | null;
  sourceWarehouseId: string | null;
  destinationWarehouseId: string | null;
  transactionDate: string;
  referenceNumber: string | null;
  notes: string | null;
  items: TransactionForEditItemDTO[];
};

export type TransactionForEditItemDTO = {
  itemId: string;
  nameSnapshot: string;
  unitOfMeasureSnapshot: string;
  quantity: number;
};

export type EditTransactionRequest = {
  transactionId: string;
  type: TransactionTypeValue;
  clientId?: string | null;
  sourceWarehouseId?: string | null;
  destinationWarehouseId?: string | null;
  transactionDate: string;
  referenceNumber?: string | null;
  notes?: string | null;
  items: {
    itemId: string;
    quantity: number;
  }[];
};
