using System.Collections.Generic;
using System.Linq;
using MaSV_MyStore.Models;
using PagedList;

namespace MaSV_MyStore.Models.ViewModel
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public IEnumerable<CartItem> Items => items;

        public List<IGrouping<string, CartItem>> GroupedItems => items.GroupBy(i => i.Category ?? "Khác").ToList();

        public IPagedList<Product> SimilarProducts { get; set; }

        public void AddItem(CartItem item)
        {
            var existing = items.FirstOrDefault(i => i.ProductID == item.ProductID);
            if (existing != null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                items.Add(item);
            }
        }

        public void RemoveItem(int productId)
        {
            items.RemoveAll(i => i.ProductID == productId);
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var item = items.FirstOrDefault(i => i.ProductID == productId);
            if (item != null)
            {
                if (quantity <= 0)
                    RemoveItem(productId);
                else
                    item.Quantity = quantity;
            }
        }

        public void Clear()
        {
            items.Clear();
        }

        public decimal TotalAmount => items.Sum(i => i.TotalPrice);
        public int TotalItems => items.Sum(i => i.Quantity);
    }
}
