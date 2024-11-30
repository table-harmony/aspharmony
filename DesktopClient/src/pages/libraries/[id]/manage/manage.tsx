import { useState, useEffect } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import {
  Library,
  getLibrary,
  promoteMember,
  demoteMember,
  removeMember,
} from "@/lib/libraries";
import { useUserStore } from "@/lib/userStore";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  Pencil,
  Plus,
  Shield,
  ShieldOff,
  Trash2,
  UserMinus,
} from "lucide-react";
import { toast } from "sonner";
import { scrollToTop } from "@/lib/utils";
import { Pagination } from "@/components/ui/pagination";
import { Input } from "@/components/ui/input";

export function ManageLibrary() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [library, setLibrary] = useState<Library | null>(null);
  const user = useUserStore((state) => state.user);
  const [isLoading, setIsLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [search, setSearch] = useState("");
  const itemsPerPage = 3;

  const userMembership = library?.members.find((m) => m.user.id === user?.id);
  const isManager = userMembership?.role === "Manager";

  useEffect(() => {
    if (!id || !user) return;

    const fetchLibrary = async () => {
      try {
        const data = await getLibrary(parseInt(id!));
        setLibrary(data);
      } catch {
        toast.error("Failed to load library details");
      }
    };

    fetchLibrary();
  }, [id, user]);

  const fetchLibrary = async () => {
    try {
      const data = await getLibrary(parseInt(id!));
      setLibrary(data);
    } catch {
      toast.error("Failed to load library details");
    }
  };

  const handlePromote = async (userId: string) => {
    if (!id || !isManager) return;
    try {
      setIsLoading(true);
      await promoteMember(parseInt(id), userId);
      await fetchLibrary();
      toast.success("Member promoted to manager");
    } catch {
      toast.error("Failed to promote member");
    } finally {
      setIsLoading(false);
    }
  };

  const handleDemote = async (userId: string) => {
    if (!id || !isManager) return;
    try {
      setIsLoading(true);
      await demoteMember(parseInt(id), userId);
      await fetchLibrary();
      toast.success("Manager demoted to member");
    } catch {
      toast.error("Failed to demote manager");
    } finally {
      setIsLoading(false);
    }
  };

  const handleRemove = async (userId: string) => {
    if (!id || !isManager) return;
    try {
      setIsLoading(true);
      await removeMember(parseInt(id), userId);
      await fetchLibrary();
      toast.success("Member removed from library");
    } catch {
      toast.error("Failed to remove member");
    } finally {
      setIsLoading(false);
    }
  };

  const filteredMembers =
    library?.members.filter(
      (member) =>
        member.user.username.toLowerCase().includes(search.toLowerCase()) ||
        member.user.email.toLowerCase().includes(search.toLowerCase())
    ) || [];

  const totalPages = Math.ceil(filteredMembers.length / itemsPerPage);
  const paginatedMembers = filteredMembers.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  useEffect(() => {
    setCurrentPage(1);
  }, [search]);

  if (!isManager) {
    navigate(`/libraries/${id}`);
    return null;
  }

  return (
    <div className="space-y-8">
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl font-bold">Manage Library</CardTitle>
          <CardDescription>Manage members and library settings</CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="flex gap-4">
            <Button asChild>
              <Link to={`/libraries/${id}/add-books`} onClick={scrollToTop}>
                <Plus className="h-4 w-4" />
                Add books
              </Link>
            </Button>
            <Button variant="outline" asChild>
              <Link to={`/libraries/${id}/edit`} onClick={scrollToTop}>
                <Pencil className="h-4 w-4" />
                Edit
              </Link>
            </Button>
            <Button variant="destructive" asChild>
              <Link to={`/libraries/${id}/delete`} onClick={scrollToTop}>
                <Trash2 className="h-4 w-4" />
                Delete
              </Link>
            </Button>
          </div>

          <Card>
            <CardHeader>
              <CardTitle>Members</CardTitle>
              <CardDescription>
                Manage library members and their roles
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex items-center space-x-2">
                <Input
                  placeholder="Search members..."
                  value={search}
                  onChange={(e) => setSearch(e.target.value)}
                  className="max-w-sm"
                />
              </div>

              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Username</TableHead>
                    <TableHead>Email</TableHead>
                    <TableHead>Role</TableHead>
                    <TableHead className="text-right">Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {paginatedMembers.map((member) => (
                    <TableRow key={member.id}>
                      <TableCell>{member.user.username}</TableCell>
                      <TableCell>{member.user.email}</TableCell>
                      <TableCell>{member.role}</TableCell>
                      <TableCell className="text-right">
                        {member.user.id !== user?.id && (
                          <div className="flex justify-end gap-2">
                            {member.role === "Member" ? (
                              <Button
                                variant="outline"
                                size="sm"
                                onClick={() => handlePromote(member.user.id)}
                                disabled={isLoading}
                              >
                                <Shield className="h-4 w-4" />
                                <span className="sr-only">Promote</span>
                              </Button>
                            ) : (
                              <Button
                                variant="outline"
                                size="sm"
                                onClick={() => handleDemote(member.user.id)}
                                disabled={isLoading}
                              >
                                <ShieldOff className="h-4 w-4" />
                                <span className="sr-only">Demote</span>
                              </Button>
                            )}
                            <Button
                              variant="outline"
                              size="sm"
                              onClick={() => handleRemove(member.user.id)}
                              disabled={isLoading}
                            >
                              <UserMinus className="h-4 w-4" />
                              <span className="sr-only">Remove</span>
                            </Button>
                          </div>
                        )}
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>

              <div className="flex items-center justify-between">
                <div className="text-sm text-muted-foreground">
                  Showing {paginatedMembers.length} of {filteredMembers.length}{" "}
                  members
                </div>
                <Pagination
                  currentPage={currentPage}
                  totalPages={totalPages}
                  onPageChange={setCurrentPage}
                />
              </div>
            </CardContent>
          </Card>
        </CardContent>
      </Card>
    </div>
  );
}
