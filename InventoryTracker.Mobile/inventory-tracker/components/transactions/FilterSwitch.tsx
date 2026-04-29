import { StyleSheet, Switch, Text, View } from 'react-native';

type Props = {
  label: string;
  value: boolean;
  onChange: (value: boolean) => void;
};

export function FilterSwitch({ label, value, onChange }: Props) {
  return (
    <View style={styles.row}>
      <Text style={styles.label}>{label}</Text>
      <Switch value={value} onValueChange={onChange} />
    </View>
  );
}

const styles = StyleSheet.create({
  row: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  label: {
    fontSize: 16,
    color: '#041b3c',
    fontWeight: '500',
  },
});