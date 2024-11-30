import { useState, useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
import { Book, getBooks } from "@/lib/books";
import { Library, getLibrary, addBookToLibrary } from "@/lib/libraries";
import { useUserStore } from "@/lib/userStore";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { ArrowLeft, Library as LibraryIcon, Plus } from "lucide-react";
import { toast } from "sonner";
import { scrollToTop } from "@/lib/utils";
import { Pagination } from "@/components/ui/pagination";
import { Search } from "lucide-react";

export function AddBooksToLibrary({ id }: { id: number }) {
  const navigate = useNavigate();
  const user = useUserStore((state) => state.user);
  const [isLoading, setIsLoading] = useState(true);
  const [library, setLibrary] = useState<Library | null>(null);
  const [books, setBooks] = useState<Book[]>([]);
  const [search, setSearch] = useState("");
  const [selectedBooks, setSelectedBooks] = useState<{ [key: number]: number }>(
    {}
  );
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 3;

  useEffect(() => {
    setCurrentPage(1);
  }, [search]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [libraryData, booksData] = await Promise.all([
          getLibrary(id),
          getBooks(),
        ]);
        setLibrary(libraryData);
        setBooks(booksData);
      } catch (error) {
        console.error("Error fetching data:", error);
        toast.error("Failed to load data");
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, [id]);

  const userMembership = library?.members.find((m) => m.user.id === user?.id);
  const isManager = userMembership?.role === "Manager";

  if (!isManager) {
    navigate(`/libraries/${id}`);
    return null;
  }

  const filteredBooks = books.filter((book) => {
    const isInLibrary = library?.books.some((lb) => lb.book.id === book.id);
    const matchesSearch = book.metadata.title
      .toLowerCase()
      .includes(search.toLowerCase());

    return (library?.allow_copies || !isInLibrary) && matchesSearch;
  });

  const totalPages = Math.ceil(filteredBooks.length / itemsPerPage);
  const paginatedBooks = filteredBooks.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  const handleCopiesChange = (bookId: number, copies: string) => {
    const numCopies = parseInt(copies);
    if (numCopies > 0) {
      setSelectedBooks({ ...selectedBooks, [bookId]: numCopies });
    } else {
      const newSelected = { ...selectedBooks };
      delete newSelected[bookId];
      setSelectedBooks(newSelected);
    }
  };

  const handleAddBooks = async () => {
    try {
      setIsLoading(true);

      for (const [bookId, copies] of Object.entries(selectedBooks)) {
        for (let i = 0; i < copies; i++) {
          await addBookToLibrary(id, parseInt(bookId));
        }
      }

      toast.success("Books added successfully");
      navigate(`/libraries/${id}`);
    } catch (error) {
      console.error("Error adding books:", error);
      toast.error("Failed to add books");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="space-y-8">
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <div>
              <CardTitle className="text-2xl font-bold">Add Books</CardTitle>
              <CardDescription>Add books to {library?.name}</CardDescription>
            </div>
            <Button variant="outline" asChild>
              <Link to={`/libraries/${id}`} onClick={scrollToTop}>
                <ArrowLeft className="h-4 w-4" />
                Back
              </Link>
            </Button>
          </div>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="flex items-center space-x-2">
            <Search className="h-4 w-4 text-muted-foreground" />
            <Input
              placeholder="Search books..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="max-w-sm"
            />
          </div>

          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Title</TableHead>
                <TableHead>Author</TableHead>
                {library?.allow_copies && <TableHead>Copies</TableHead>}
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {paginatedBooks.map((book) => (
                <TableRow key={book.id}>
                  <TableCell>{book.metadata.title}</TableCell>
                  <TableCell>{book.author.username}</TableCell>
                  {library?.allow_copies && (
                    <TableCell>
                      <Select
                        value={selectedBooks[book.id]?.toString() || "0"}
                        onValueChange={(value) =>
                          handleCopiesChange(book.id, value)
                        }
                      >
                        <SelectTrigger className="w-20">
                          <SelectValue />
                        </SelectTrigger>
                        <SelectContent>
                          {Array.from({ length: 10 }, (_, i) => (
                            <SelectItem key={i} value={i.toString()}>
                              {i}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </TableCell>
                  )}
                  <TableCell className="text-right">
                    <Button
                      variant="outline"
                      size="sm"
                      onClick={() => {
                        if (library?.allow_copies) {
                          handleCopiesChange(book.id, "1");
                        } else {
                          handleCopiesChange(
                            book.id,
                            selectedBooks[book.id] ? "0" : "1"
                          );
                        }
                      }}
                    >
                      <Plus className="h-4 w-4" />
                      {library?.allow_copies
                        ? "Select"
                        : selectedBooks[book.id]
                        ? "Selected"
                        : "Select"}
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          <div className="flex items-center justify-between">
            <div className="text-sm text-muted-foreground">
              Showing {paginatedBooks.length} of {filteredBooks.length} books
            </div>
            <Pagination
              currentPage={currentPage}
              totalPages={totalPages}
              onPageChange={setCurrentPage}
            />
          </div>

          {Object.keys(selectedBooks).length > 0 && (
            <div className="flex justify-end">
              <Button onClick={handleAddBooks} disabled={isLoading}>
                <LibraryIcon className="h-4 w-4" />
                Add Selected Books
              </Button>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
