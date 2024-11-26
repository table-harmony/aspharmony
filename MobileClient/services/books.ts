import api from "./api";
import { API_ENDPOINTS } from "@/config/api";

export interface Book {
  id: string;
  title: string;
  description: string;
  chapters: Chapter[];
}

export interface Chapter {
  title: string;
  content: string;
}

export const BooksService = {
  async getBooks() {
    const response = await api.get<Book[]>(API_ENDPOINTS.BOOKS.LIST);
    return response.data;
  },

  async getBook(id: string) {
    const response = await api.get<Book>(API_ENDPOINTS.BOOKS.DETAILS(id));
    return response.data;
  },

  async createBook(book: Omit<Book, "id">) {
    const response = await api.post<Book>(API_ENDPOINTS.BOOKS.CREATE, book);
    return response.data;
  },

  async updateBook(id: string, book: Omit<Book, "id">) {
    const response = await api.put<Book>(API_ENDPOINTS.BOOKS.UPDATE(id), book);
    return response.data;
  },

  async deleteBook(id: string) {
    await api.delete(API_ENDPOINTS.BOOKS.DELETE(id));
  },

  async generateChapters(
    title: string,
    description: string,
    existingChapters: Chapter[]
  ) {
    const response = await api.post<Chapter[]>(
      API_ENDPOINTS.BOOKS.GENERATE_CHAPTERS,
      {
        title,
        description,
        chapters: existingChapters,
      }
    );
    return response.data;
  },
};
