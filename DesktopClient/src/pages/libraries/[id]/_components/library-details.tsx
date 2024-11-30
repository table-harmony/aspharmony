import { Library, joinLibrary, removeMember } from "@/lib/libraries";
import { useUserStore } from "@/lib/userStore";
import { Link, useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { BookCard } from "@/pages/books/_components/book-card";
import { Settings, UserMinus, UserPlus } from "lucide-react";
import { toast } from "sonner";
import { scrollToTop } from "@/lib/utils";

export function LibraryDetails({ library }: { library: Library }) {
  const user = useUserStore((state) => state.user);
  const userMembership = library.members.find((m) => m.user.id === user?.id);
  const isManager = userMembership?.role === "Manager";
  const navigate = useNavigate();

  const handleJoin = async () => {
    if (!user) {
      toast.error("You must be logged in to join a library");
      return;
    }

    try {
      await joinLibrary(library.id, user.id);
      toast.success("Successfully joined the library");

      navigate(`/libraries`);
    } catch {
      toast.error("Failed to join the library");
    }
  };

  const handleLeave = async () => {
    if (!user) return;

    try {
      await removeMember(library.id, user.id);
      toast.success("Successfully left the library");

      navigate("/libraries");
    } catch {
      toast.error("Failed to leave the library");
    }
  };

  return (
    <div className="flex flex-col gap-8">
      <div className="flex flex-col gap-6">
        <div className="space-y-4">
          <h1 className="text-3xl font-bold">{library.name}</h1>
          <p className="text-lg text-muted-foreground">
            {library.allow_copies ? "Allows copies" : "No copies allowed"}
          </p>
          <div className="flex gap-4 pt-2">
            {!userMembership ? (
              <Button onClick={handleJoin}>
                <UserPlus className="h-4 w-4" />
                Join
              </Button>
            ) : (
              <>
                <Button variant="destructive" onClick={handleLeave}>
                  <UserMinus className="h-4 w-4" />
                  Leave
                </Button>
                {isManager && (
                  <Button asChild variant="outline">
                    <Link
                      to={`/libraries/${library.id}/manage`}
                      onClick={scrollToTop}
                    >
                      <Settings className="h-4 w-4" />
                      Manage
                    </Link>
                  </Button>
                )}
              </>
            )}
          </div>
        </div>
      </div>

      <Separator />

      <section className="space-y-6">
        <h2 className="text-2xl font-semibold">Books</h2>
        {library.books.length === 0 ? (
          <Card>
            <CardContent className="flex flex-col items-center gap-4 p-6">
              <p className="text-muted-foreground">
                No books in this library yet
              </p>
              {isManager && (
                <Button asChild>
                  <Link
                    to={`/libraries/${library.id}/add-books`}
                    onClick={scrollToTop}
                  >
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
