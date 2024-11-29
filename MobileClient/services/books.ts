import { api } from "./api";
import { User } from "./auth";

export type ServerType = {
  id: number;
  name: string;
  display_name: string;
};

export type AudioBook = {
  id: number;
  book_id: number;
  audio_url: string;
};

export type Chapter = {
  index: number;
  title: string;
  content: string;
};

export type BookMetadata = {
  title: string;
  description: string;
  image_url: string;
  chapters: Chapter[];
};

export type Book = {
  id: number;
  server: number;
  author: User;
  metadata: BookMetadata;
  audio_books: AudioBook[];
};

export type CreateBookDto = Omit<Book, "id" | "author"> & {
  author_id: string;
};

export async function getBooks(): Promise<Book[]> {
  const response = await api.get("/books");
  return response.data as Book[];
}

export async function getBookById(id: number): Promise<Book> {
  const response = await api.get(`/books/${id}`);
  return response.data as Book;
}

export async function createBook(book: CreateBookDto): Promise<Book> {
  const response = await api.post("/books", book);
  return response.data as Book;
}

export async function updateBook(book: Book): Promise<void> {
  await api.put(`/books/${book.id}`, book);
}

export async function deleteBook(id: number): Promise<void> {
  await api.delete(`/books/${id}`);
}

export async function getServers(): Promise<ServerType[]> {
  const response = await api.get("/books/servers");
  return response.data as ServerType[];
}
