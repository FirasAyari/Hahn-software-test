import axios from "axios";

export interface ApiEntry {
  id: number;
  api: string;
  description: string;
  auth: string;
  https: boolean;
  cors: string;
  link: string;
  category: string;
}

export async function fetchApiEntries(): Promise<ApiEntry[]> {
  try {
    const response = await axios.get<ApiEntry[]>("http://localhost:5299/api/ApiEntries");
    return response.data;
  } catch (error) {
    console.error("Error fetching API entries:", error);
    return [];
  }
}
