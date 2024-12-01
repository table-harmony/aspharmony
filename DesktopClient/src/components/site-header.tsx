import { Link } from "react-router-dom";
import {
  BookIcon,
  BookOpenText,
  LibraryIcon,
  LogInIcon,
  UserPlusIcon,
} from "lucide-react";
import { useUserStore } from "@/lib/userStore";
import { Button } from "@/components/ui/button";
import { scrollToTop } from "@/lib/utils";
import { LogoutButton } from "./logout-button";

export function SiteHeader() {
  const user = useUserStore((state) => state.user);

  return (
    <header className="top-0 z-50 w-full border-b border-border/40 backdrop-blur supports-[backdrop-filter]:bg-background/60">
      <div className="container max-w-6xl mx-auto px-4 sm:px-6 py-2 sm:py-4">
        <div className="flex h-14 items-center justify-between">
          <div className="flex items-center gap-2">
            <Link to="/" className="flex items-center gap-2">
              <BookOpenText className="h-5 w-5" />
              <span className="text-lg font-bold">AspHarmony</span>
            </Link>
            {user && (
              <div className="flex items-center gap-4 ml-6">
                <Button asChild>
                  <Link to="/books" onClick={scrollToTop}>
                    <BookIcon className="h-4 w-4" />
                    Books
                  </Link>
                </Button>
                <Button variant="outline" asChild>
                  <Link to="/libraries" onClick={scrollToTop}>
                    <LibraryIcon className="h-4 w-4" />
                    Libraries
                  </Link>
                </Button>
              </div>
            )}
          </div>
          {!user ? (
            <div className="flex items-center gap-4 ml-6">
              <Button asChild>
                <Link to="/login">
                  <LogInIcon className="h-4 w-4" />
                  Login
                </Link>
              </Button>
              <Button variant="outline" asChild>
                <Link to="/register">
                  <UserPlusIcon className="h-4 w-4" />
                  Register
                </Link>
              </Button>
            </div>
          ) : (
            <LogoutButton variant="secondary" />
          )}
        </div>
      </div>
    </header>
  );
}
