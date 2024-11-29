import { View, StyleSheet } from "react-native";
import { Button, Text } from "react-native-paper";

type Props = {
  message: string;
  onRetry: () => void;
};

export function ErrorScreen({ message, onRetry }: Props) {
  return (
    <View style={styles.container}>
      <Text variant="bodyLarge" style={styles.message}>
        {message}
      </Text>
      <Button mode="contained" onPress={onRetry}>
        Retry
      </Button>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    padding: 16,
  },
  message: {
    marginBottom: 16,
    textAlign: "center",
  },
});
