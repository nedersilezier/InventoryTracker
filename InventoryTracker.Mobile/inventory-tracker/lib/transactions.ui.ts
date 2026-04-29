import { Ionicons } from "@expo/vector-icons";

//transaction types -> UI representation mapper
export const TYPE_UI: Record<string, { label: string; color: string; bg: string; icon: keyof typeof Ionicons.glyphMap }> = {
  Adjustment: {
    label: 'Adjustment',
    color: '#0052cc',
    bg: '#e8edff',
    icon: 'options-outline',
  },
  Return: {
    label: 'Return',
    color: '#006844',
    bg: '#e7f5ed',
    icon: 'arrow-undo-outline',
  },
  Issue: {
    label: 'Issue',
    color: '#ba1a1a',
    bg: '#ffdad6',
    icon: 'arrow-redo-outline',
  },
  Transfer: {
    label: 'Transfer',
    color: '#5c5f60',
    bg: '#dee0e2',
    icon: 'swap-horizontal-outline',
  },
};