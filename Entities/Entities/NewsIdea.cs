using System;
using Entities.Entities.Identity;
using Entities.Enums;

namespace Entities.Entities
{
    /// <summary>
    /// Идея для статьи
    /// </summary>
    public class NewsIdea
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Автор идеи
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Дата удаления
        /// </summary>
        public DateTime? DeletedDt { get; set; }

        /// <summary>
        /// Пользователь, удаливший идею
        /// </summary>
        public Guid? DeleteUserId { get; set; }

        /// <summary>
        /// Причина удаления/отклонения
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public NewsIdeaStatus Status { get; set; }

        public virtual User User { get; set; }
        public virtual User DeleteUser { get; set; }
    }
}