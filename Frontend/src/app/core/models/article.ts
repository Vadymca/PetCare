import { Category } from './category';
import { User } from './user';

export type ArticleStatus = 'draft' | 'published' | 'archived';
export interface Article {
  id: string;
  title: string;
  content: string;
  categoryId: string;
  category?: Category;
  authorId: string;
  author?: User;
  status: ArticleStatus;
  slug?: string;
  createdAt: string;
  updatedAt: string;
}
