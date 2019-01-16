using JustBasic.Data.Infrastructure;
using JustBasic.Data.Repositories;
using JustBasic.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBasic.Service
{
    public interface IOrderService
    {
        Order Create(ref Order order, List<OrderDetail> orderDetails);

        void UpdateStatus(int orderId);

        void Save();

        IEnumerable<Order> GetListOrderByCustomerId(string customerId);

        IEnumerable<Order> GetAll();
    }

    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public Order Create(ref Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }
                _unitOfWork.Commit();
                return order;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateStatus(int orderId)
        {
            var order = _orderRepository.GetSingleById(orderId);
            order.Status = true;
            _orderRepository.Update(order);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Order> GetListOrderByCustomerId(string customerId)
        {
            var LstOrder = _orderRepository.GetMulti(x => x.CustomerId == customerId, new string[] { "OrderDetails" });
            return LstOrder;
        }

        public IEnumerable<Order> GetAll()
        {
            var list = _orderRepository.GetAll(new string[] { "OrderDetails" });
            return list;
        }
    }
}
