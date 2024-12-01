import { Library } from "@/lib/libraries";
import { useUserStore } from "@/lib/userStore";
import { Link } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { BookCard } from "@/pages/books/_components/book-card";
import { scrollToTop } from "@/lib/utils";
import emptyState from "@/assets/empty.svg";
import { Plus } from "lucide-react";

export function LibraryDetails({ library }: { library: Library }) {
  const user = useUserStore((state) => state.user);
  const userMembership = library.members.find((m) => m.user.id === user?.id);
  const isManager = userMembership?.role === "Manager";

  return (
    <div className="flex flex-col gap-8">
      <section className="space-y-6">
        <h2 className="text-2xl font-semibold">Books</h2>
        {library.books.length === 0 ? (
          <Card>
            <CardContent className="flex flex-col items-center gap-4 p-6">
              <p className="text-muted-foreground">
                No books in this library yet
              </p>
              <img src={emptyState} width={200} height={200} alt="No books" />
              {isManager && (
                <Button size="lg" asChild>
                  <Link
                    to={`/libraries/${library.id}/add-books`}
                    onClick={scrollToTop}
                  >
                    <Plus className="mr-2 h-4 w-4" />
                    Add Books
                  </Link>
                </Button>
              )}
            </CardContent>
          </Card>
        ) : (
          <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
            {Array.from(
              new Map(library.books.map((lb) => [lb.book.id, lb.book])).values()
            ).map((book) => (
              <BookCard key={book.id} book={book} />
            ))}
          </div>
        )}
      </section>
    </div>
  );
}
