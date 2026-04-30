import { StyleSheet, Text } from 'react-native';

// Reusable section heading used across create-transaction steps.
export function SectionTitle({ title }: { title: string }) {
  return <Text style={styles.sectionTitle}>{title}</Text>;
}

const styles = StyleSheet.create({
  sectionTitle: {
    marginTop: 18,
    marginBottom: 10,
    fontSize: 18,
    fontWeight: '700',
    color: '#041b3c',
  },
});