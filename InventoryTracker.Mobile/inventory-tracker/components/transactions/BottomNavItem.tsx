import { Ionicons } from '@expo/vector-icons';
import { Pressable, StyleSheet, Text } from 'react-native';

type Props = {
  icon: keyof typeof Ionicons.glyphMap;
  label: string;
  active: boolean;
  onOpen: () => void;
};

export function BottomNavItem({ icon, label, active, onOpen }: Props) {
  return (
    <Pressable style={[styles.navItem, active && styles.navItemActive]} onPress={ onOpen }>
      <Ionicons name={icon} size={26} color={active ? '#0052cc' : '#64748b'} />
      <Text style={[styles.navText, active && styles.navTextActive]}>
        {label}
      </Text>
    </Pressable>
  );
}

const styles = StyleSheet.create({
  navItem: {
    width: 88,
    height: 56,
    alignItems: 'center',
    justifyContent: 'center',
    borderRadius: 12,
    gap: 2,
  },
  navItemActive: {
    backgroundColor: '#e8edff',
  },
  navText: {
    fontSize: 12,
    color: '#64748b',
    fontWeight: '500',
  },
  navTextActive: {
    color: '#0052cc',
    fontWeight: '700',
  },
});