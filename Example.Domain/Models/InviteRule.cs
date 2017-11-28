/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/10/25 10:48:00
 *  FileName:InviteRule.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Example.Domain.Models
{
    public class InviteRule // 邀请码附加规则
    {
        public int Id { get; set; }

        //public int Count { get; set; } // 多少个邀请码激活完成后延长哪种增值服务

        //public int ExtendServiceCount { get; set; } // 延长服务 比如 12个月
        public int UserId { get; set; } // 用户Id

        public int ServiceId { get; set; } // 增值服务Id

        public int ServicdeId { get; set; } // 增值服务套餐Id

        public string ChildCodeId { get; set; } //子邀请码
    }


    // 用户激活邀请码的时候, 根据这个邀请码 去查询 生成这个邀请码的源用户Id,再得到邀请码附加规则,
    // 根据邀请码附加规则 就可以得到需要激活几个邀请码,开通哪个增值服务... end
    public class InviteRuleDetail
    {
        public int Id { get; set; }

        public int Count { get; set; } // 多少个邀请码激活完成后延长哪种增值服务

        public int ServiceId { get; set; } // 增值服务Id
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
    }
    public class Service // 增值服务
    {
        public int Id { get; set; }
        public string Name { get; set; } // 服务名称 1x5 5x20 什么的...
        // 其他... 
    }

    public class InviteCode
    {
        public int Id { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        [StringLength(10), Required]
        [Index("IX_InviteCode_Unique", IsUnique = true)]
        public string Code { get; set; }
        /// <summary>
        /// 舟大师版本
        /// </summary>
        [StringLength(10)]
        public string Version { get; set; }
        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime GenerateTime { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UseTime { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public SourceTypeEnum SourceType { get; set; }
        /// <summary>
        /// 来源用户Id
        /// </summary>
        public int SourceUserId { get; set; }
        /// <summary>
        /// 使用用户Id
        /// </summary>
        public int UseUserId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public InviteState State { get; set; }
        /// <summary>
        /// 使用Ip
        /// </summary>
        [StringLength(40)]
        public string UseIpAddress { get; set; }
        /// <summary>
        /// 使用MAC地址
        /// </summary>
        [StringLength(40)]
        public string UseMacAddress { get; set; }

        //public int InviteRuleId { get; set; } // 邀请码附加规则Id

        //[ForeignKey("InviteRuleId")]
        //public virtual InviteRule InviteRule { get; set; }
    }

    [Flags]
    public enum SourceTypeEnum
    {
        /// <summary>
        /// X 代表系统后台
        /// </summary>
        X = 0,
        /// <summary>
        /// Y 代表用户
        /// </summary>
        Y = 2,
        /// <summary>
        /// D 代表代理商
        /// </summary>
        D = 1
    }
    public enum InviteState
    {
        /// <summary>
        /// 未激活
        /// </summary>
        Unactivated = 0,
        /// <summary>
        /// 已激活
        /// </summary>
        Activated = 1
    }
}
