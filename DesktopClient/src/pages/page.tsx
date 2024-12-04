import { useUserStore } from "@/lib/userStore";
import { Link } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import {
  BookOpenText,
  Headphones,
  Library,
  LogIn,
  PenSquare,
  UserPlus,
} from "lucide-react";

export default function HomePage() {
  const user = useUserStore((state) => state.user);

  return (
    <div>
      <section className="flex flex-col items-center gap-6 px-6 py-12 text-center">
        <div className="rounded-full bg-primary/10 p-6">
          <BookOpenText className="h-12 w-12 text-primary" />
        </div>
        <div className="space-y-3">
          <h1 className="text-4xl font-bold tracking-tighter sm:text-5xl">
            Welcome to AspHarmony
          </h1>
          <p className="mx-auto max-w-[600px] text-muted-foreground">
            Your personal library in the cloud
          </p>
        </div>

        <div className="flex w-full max-w-sm gap-4">
          {user ? (
            <>
              <Button asChild className="flex-1">
                <Link to="/books">
                  <BookOpenText className="mr-2 h-4 w-4" />
                  Books
                </Link>
              </Button>
              <Button asChild variant="outline" className="flex-1">
                <Link to="/libraries">
                  <Library className="mr-2 h-4 w-4" />
                  Libraries
                </Link>
              </Button>
            </>
          ) : (
            <>
              <Button asChild className="flex-1">
                <Link to="/login">
                  <LogIn className="mr-2 h-4 w-4" />
                  Login
                </Link>
              </Button>
              <Button asChild variant="outline" className="flex-1">
                <Link to="/register">
                  <UserPlus className="mr-2 h-4 w-4" />
                  Register
                </Link>
              </Button>
            </>
          )}
        </div>
      </section>

      <Separator className="my-12" />

      <section className="px-6 pb-12">
        <div className="text-center">
          <h2 className="text-3xl font-bold tracking-tight">Features</h2>
        </div>

        <div className="mx-auto mt-8 grid max-w-5xl gap-6 sm:grid-cols-2 lg:grid-cols-3">
          <Card>
            <CardContent className="flex flex-col items-center gap-4 p-6">
              <div className="rounded-full bg-primary/10 p-3">
                <BookOpenText className="h-6 w-6 text-primary" />
              </div>
              <div className="space-y-2 text-center">
                <h3 className="font-semibold">Digital Library</h3>
                <p className="text-sm text-muted-foreground">
                  Store and organize your books in one place
                </p>
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardContent className="flex flex-col items-center gap-4 p-6">
              <div className="rounded-full bg-primary/10 p-3">
                <Headphones className="h-6 w-6 text-primary" />
              </div>
              <div className="space-y-2 text-center">
                <h3 className="font-semibold">Audio Support</h3>
                <p className="text-sm text-muted-foreground">
                  Listen to your books on the go
                </p>
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardContent className="flex flex-col items-center gap-4 p-6">
              <div className="rounded-full bg-primary/10 p-3">
                <PenSquare className="h-6 w-6 text-primary" />
              </div>
              <div className="space-y-2 text-center">
                <h3 className="font-semibold">Create & Share</h3>
                <p className="text-sm text-muted-foreground">
                  Write and publish your own stories
                </p>
              </div>
            </CardContent>
          </Card>
        </div>
      </section>
    </div>
  );
}
