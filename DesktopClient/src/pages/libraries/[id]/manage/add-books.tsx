import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Book, getBooks } from "@/lib/books";
import { Library, addBookToLibrary } from "@/lib/libraries";
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
import {
  Library as LibraryIcon,
  Loader2,
  MinusIcon,
  PlusIcon,
} from "lucide-react";
import { toast } from "sonner";
import { Pagination } from "@/components/ui/pagination";

export function AddBooks({ library }: { library: Library }) {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
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
        const [booksData] = await Promise.all([getBooks()]);
        setBooks(booksData);
      } catch (error) {
        console.error("Error fetching data:", error);
        toast.error("Failed to load data");
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, []);

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
          await addBookToLibrary(library.id, parseInt(bookId));
        }
      }

      toast.success("Books added successfully");
      navigate(`/libraries/${library.id}`);
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
          <CardTitle className="text-2xl font-bold">Add Books</CardTitle>
          <CardDescription>
            Select books to add to "{library?.name}"
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="flex justify-between space-x-2">
            <Input
              placeholder="Search books..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="max-w-sm"
            />
            {Object.keys(selectedBooks).length > 0 && (
              <Button onClick={handleAddBooks} disabled={isLoading}>
                {isLoading ? (
                  <>
                    <Loader2 className="h-4 w-4 animate-spin" />
                    Adding books...
                  </>
                ) : (
                  <>
                    <LibraryIcon className="h-4 w-4" />
                    Add books
                  </>
                )}
              </Button>
            )}
          </div>

          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Title</TableHead>
                <TableHead>Author</TableHead>
                {library?.allow_copies ? (
                  <TableHead>Copies</TableHead>
                ) : (
                  <TableHead>Select</TableHead>
                )}
              </TableRow>
            </TableHeader>
            <TableBody>
              {paginatedBooks.map((book) => (
                <TableRow key={book.id}>
                  <TableCell className="font-medium">
                    {book.metadata.title}
                  </TableCell>
                  <TableCell>{book.author.username}</TableCell>
                  {library?.allow_copies ? (
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
                  ) : (
                    <TableCell>
                      {(selectedBooks[book.id]?.toString() === "0" ||
                        !selectedBooks[book.id]) && (
                        <Button
                          variant="outline"
                          size="sm"
                          onClick={() => handleCopiesChange(book.id, "1")}
                        >
                          <PlusIcon className="h-4 w-4" />
                          Add
                        </Button>
                      )}
                      {selectedBooks[book.id]?.toString() !== "0" &&
                        selectedBooks[book.id] && (
                          <Button
                            variant="destructive"
                            size="sm"
                            onClick={() => handleCopiesChange(book.id, "0")}
                          >
                            <MinusIcon className="h-4 w-4" />
                            Remove
                          </Button>
                        )}
                    </TableCell>
                  )}
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
        </CardContent>
      </Card>
    </div>
  );
}
