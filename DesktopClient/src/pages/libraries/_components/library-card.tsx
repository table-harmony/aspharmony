import { Library } from "@/lib/libraries";
import { Link } from "react-router-dom";
import { Card, CardContent, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { BookIcon, Users } from "lucide-react";
import { scrollToTop } from "@/lib/utils";

export function LibraryCard({ library }: { library: Library }) {
  return (
    <Card className="flex flex-col">
      <CardContent className="flex flex-col flex-1 gap-2.5 p-4">
        <div className="space-y-1">
          <h3 className="font-semibold text-lg leading-none tracking-tight">
            {library.name}
          </h3>
          <p className="text-sm text-muted-foreground">
            {library.allow_copies ? "Allows copies" : "No copies allowed"}
          </p>
        </div>
      </CardContent>
      <CardFooter className="p-4 pt-0">
        <div className="flex w-full items-center justify-between gap-2">
          <Button variant="outline" size="sm" asChild>
            <Link to={`/libraries/${library.id}`} onClick={scrollToTop}>
              <BookIcon className="h-4 w-4" />
              View
            </Link>
          </Button>
          <div className="flex items-center gap-1 text-sm text-muted-foreground">
            <Users className="h-4 w-4" />
            <span>{library.members.length} members</span>
          </div>
        </div>
      </CardFooter>
    </Card>
  );
}
