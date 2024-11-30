import { api } from "./api";

import type { Book } from "./books";
import type { User } from "./auth";

type MembershipRole = "Manager" | "Member";

export type LibraryMembership = {
  id: number;
  role: MembershipRole;
  user: User;
};

export type LibraryBook = {
  id: number;
  book_id: number;
  book: Book;
};

export type Library = {
  id: number;
  name: string;
  allow_copies: boolean;
  books: LibraryBook[];
  members: LibraryMembership[];
};

export type CreateLibraryDto = {
  name: string;
  allow_copies: boolean;
};

export type UpdateLibraryDto = {
  id: number;
  name: string;
  allow_copies: boolean;
};

export async function getLibraries(): Promise<Library[]> {
  const response = await api.get("/libraries");
  console.log("libraries", response.data);
  return response.data;
}

export async function getLibrary(id: number): Promise<Library> {
  const response = await api.get(`/libraries/${id}`);
  return response.data;
}

export async function createLibrary(
  library: CreateLibraryDto
): Promise<Library> {
  const response = await api.post("/libraries", library);
  return response.data;
}

export async function updateLibrary(library: UpdateLibraryDto): Promise<void> {
  await api.put(`/libraries/${library.id}`, library);
}

export async function deleteLibrary(id: number): Promise<void> {
  await api.delete(`/libraries/${id}`);
}

export async function addBookToLibrary(
  libraryId: number,
  bookId: number
): Promise<void> {
  await api.post(`/libraries/${libraryId}/books`, { book_id: bookId });
}

export async function removeBookFromLibrary(
  libraryId: number,
  bookId: number
): Promise<void> {
  await api.delete(`/libraries/${libraryId}/books/${bookId}`);
}

export async function joinLibrary(
  libraryId: number,
  userId: string,
  role: MembershipRole = "Member"
): Promise<LibraryMembership> {
  const response = await api.post(`/libraries/${libraryId}/members`, {
    user_id: userId,
    role,
  });
  return response.data;
}

export async function removeMember(
  libraryId: number,
  userId: string
): Promise<void> {
  await api.delete(`/libraries/${libraryId}/members/${userId}`);
}

export async function promoteMember(
  libraryId: number,
  userId: string
): Promise<void> {
  await api.post(`/libraries/${libraryId}/members/${userId}/promote`);
}

export async function demoteMember(
  libraryId: number,
  userId: string
): Promise<void> {
  await api.post(`/libraries/${libraryId}/members/${userId}/demote`);
}
