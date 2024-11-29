import React, { useEffect, useState } from "react";
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  TouchableOpacity,
  Alert,
} from "react-native";
import {
  Book,
  ServerType,
  getBookById,
  getServers,
  updateBook,
  deleteAudioBook,
} from "../services/books";
import { Picker } from "@react-native-picker/picker";
import { AudioPlayer } from "../components/AudioPlayer";

export function BookScreen({ route, navigation }) {
  const { id } = route.params;
  const [book, setBook] = useState<Book | null>(null);
  const [servers, setServers] = useState<ServerType[]>([]);
  const [selectedServer, setSelectedServer] = useState<number | null>(null);

  useEffect(() => {
    loadBook();
    loadServers();
  }, []);

  const loadBook = async () => {
    try {
      const data = await getBookById(id);
      setBook(data);
      setSelectedServer(data.server);
    } catch (error) {
      Alert.alert("Error", "Failed to load book");
    }
  };

  const loadServers = async () => {
    try {
      const data = await getServers();
      setServers(data);
    } catch (error) {
      Alert.alert("Error", "Failed to load servers");
    }
  };

  const handleServerChange = async (serverId: number) => {
    if (!book) return;

    try {
      setSelectedServer(serverId);
      const updatedBook = await updateBook({
        ...book,
        server: serverId,
      });
      setBook(updatedBook);
      Alert.alert("Success", "Server updated successfully");
    } catch (error) {
      Alert.alert("Error", "Failed to update server");
      setSelectedServer(book.server); // Reset to original value
    }
  };

  const handleDeleteAudio = async (audioId: number) => {
    try {
      await deleteAudioBook(audioId);
      // Refresh book data to show updated audio books
      loadBook();
      Alert.alert("Success", "Audio book deleted successfully");
    } catch (error) {
      Alert.alert("Error", "Failed to delete audio book");
    }
  };

  if (!book) {
    return <Text>Loading...</Text>;
  }

  return (
    <ScrollView style={styles.container}>
      {/* Book metadata */}
      <View style={styles.section}>
        <Text style={styles.title}>{book.metadata?.title}</Text>
        <Text style={styles.author}>by {book.author.username}</Text>
        <Text style={styles.description}>{book.metadata?.description}</Text>
      </View>

      {/* Server selection */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Server</Text>
        <Picker
          selectedValue={selectedServer}
          onValueChange={handleServerChange}
          style={styles.picker}
        >
          {servers.map((server) => (
            <Picker.Item
              key={server.id}
              label={server.display_name}
              value={server.id}
            />
          ))}
        </Picker>
      </View>

      {/* Audio books section */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Audio Books</Text>
        {book.audioBooks.length === 0 ? (
          <Text style={styles.noContent}>No audio books available</Text>
        ) : (
          book.audioBooks.map((audio) => (
            <View key={audio.id} style={styles.audioItem}>
              <AudioPlayer url={audio.audioUrl} />
              <TouchableOpacity
                onPress={() => handleDeleteAudio(audio.id)}
                style={styles.deleteButton}
              >
                <Text style={styles.deleteButtonText}>Delete</Text>
              </TouchableOpacity>
            </View>
          ))
        )}
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
  },
  section: {
    marginBottom: 24,
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: "bold",
    marginBottom: 8,
  },
  title: {
    fontSize: 24,
    fontWeight: "bold",
    marginBottom: 8,
  },
  author: {
    fontSize: 16,
    color: "#666",
    marginBottom: 16,
  },
  description: {
    fontSize: 16,
    lineHeight: 24,
  },
  picker: {
    backgroundColor: "#f5f5f5",
    borderRadius: 8,
  },
  audioItem: {
    flexDirection: "row",
    alignItems: "center",
    backgroundColor: "#f5f5f5",
    padding: 12,
    borderRadius: 8,
    marginBottom: 8,
  },
  deleteButton: {
    backgroundColor: "#ff4444",
    padding: 8,
    borderRadius: 4,
    marginLeft: 8,
  },
  deleteButtonText: {
    color: "white",
    fontWeight: "bold",
  },
  noContent: {
    fontStyle: "italic",
    color: "#666",
  },
});
