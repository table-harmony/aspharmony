import {
  Library,
  getLibrary,
  joinLibrary,
  removeMember,
} from "@/lib/libraries";
import { useEffect, useState } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import { LibraryDetails } from "./_components/library-details";
import { LibraryDetailsSkeleton } from "./_components/library-details-skeleton";
import { Separator } from "@/components/ui/separator";
import { Button } from "@/components/ui/button";
import {
  Library as LibraryIcon,
  PartyPopper,
  Settings,
  UserMinus,
  UserPlus,
} from "lucide-react";
import { useUserStore } from "@/lib/userStore";
import { scrollToTop } from "@/lib/utils";
import { toast } from "sonner";
import { Skeleton } from "@/components/ui/skeleton";

export default function LibraryDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [library, setLibrary] = useState<Library | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const user = useUserStore((state) => state.user);
  const userMembership = library?.members.find((m) => m.user.id === user?.id);
  const isManager = userMembership?.role === "Manager";
  const managers = library?.members.filter((m) => m.role === "Manager");

  useEffect(() => {
    if (!id) return;

    const fetchLibrary = async () => {
      try {
        const data = await getLibrary(parseInt(id));
        setLibrary(data);
      } catch (error) {
        console.error("Error fetching library:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchLibrary();
  }, [id]);

  if (isLoading) {
    return (
      <div className="py-8">
        <div className="container max-w-4xl mx-auto px-4 sm:px-6">
          <div className="flex flex-col gap-8">
            <section className="flex flex-col items-center text-center gap-8">
              <Skeleton className="w-24 h-24 rounded-full" />
              <div className="space-y-4">
                <div className="space-y-2">
                  <Skeleton className="h-8 w-64 mx-auto" />
                  <Skeleton className="h-4 w-48 mx-auto" />
                  <Skeleton className="h-4 w-32 mx-auto" />
                </div>
                <Skeleton className="h-10 w-32 mx-auto" />
              </div>
            </section>
            <Separator />
            <section className="space-y-6">
              <Skeleton className="h-[200px] w-full" />
            </section>
          </div>
        </div>
      </div>
    );
  }

  const handleJoin = async () => {
    if (!user || !id) return;
    try {
      await joinLibrary(parseInt(id), user.id);
      toast.success("Successfully joined the library");
      navigate(`/libraries`);
    } catch {
      toast.error("Failed to join the library");
    }
  };

  const handleLeave = async () => {
    if (!user || !id) return;
    try {
      await removeMember(parseInt(id), user.id);
      toast.success("Successfully left the library");
      navigate("/libraries");
    } catch {
      toast.error("Failed to leave the library");
    }
  };

  return (
    <div className="py-8">
      <div className="container max-w-4xl mx-auto px-4 sm:px-6">
        <div className="flex flex-col gap-8">
          <section className="flex flex-col items-center text-center gap-8">
            <div className="w-24 h-24 rounded-full bg-primary/10 flex items-center justify-center">
              <LibraryIcon className="h-12 w-12 text-primary" />
            </div>
            <div className="space-y-4">
              <div className="space-y-2">
                <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl">
                  {library?.name || "Library Details"}
                </h1>
                <p className="text-lg text-muted-foreground">
                  Managed by{" "}
                  {managers && managers.length > 0 ? (
                    <div className="font-semibold">
                      {managers.map((m) => m.user.username).join(", ")}
                    </div>
                  ) : (
                    <div className="items-center gap-2 inline-flex font-semibold">
                      no one <PartyPopper className="h-4 w-4" />
                    </div>
                  )}
                </p>
                <p className="text-muted-foreground">
                  {library?.allow_copies
                    ? "Allows copies"
                    : "No copies allowed"}
                </p>
              </div>
              <div className="flex gap-4 justify-center pt-4">
                {!userMembership ? (
                  <Button size="lg" onClick={handleJoin}>
                    <UserPlus className="h-4 w-4 mr-2" />
                    Join
                  </Button>
                ) : (
                  <>
                    <Button
                      size="lg"
                      variant="destructive"
                      onClick={handleLeave}
                    >
                      <UserMinus className="h-4 w-4 mr-2" />
                      Leave
                    </Button>
                    {isManager && (
                      <Button size="lg" asChild variant="outline">
                        <Link
                          to={`/libraries/${id}/manage`}
                          onClick={scrollToTop}
                        >
                          <Settings className="h-4 w-4 mr-2" />
                          Manage
                        </Link>
                      </Button>
                    )}
                  </>
                )}
              </div>
            </div>
          </section>

          <Separator />

          <section className="space-y-6">
            {library ? (
              <LibraryDetails library={library} />
            ) : (
              <LibraryDetailsSkeleton />
            )}
          </section>
        </div>
      </div>
    </div>
  );
}
