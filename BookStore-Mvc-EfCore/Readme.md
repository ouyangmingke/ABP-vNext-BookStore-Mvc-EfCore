### 学习分析   [Acme.BookStore](https://docs.abp.io/zh-Hans/abp/latest/Tutorials/Part-1?UI=MVC&DB=EF)   项目



![domain-driven-design-layers](https://raw.githubusercontent.com/abpframework/abp/rel-4.3/docs/zh-Hans/images/domain-driven-design-layers.png

## DDD  Domain Driven Design   领域驱动设计

重点：

聚合  实体   值对象

### 聚合 

**自我理解** 一个领域的核心 领域行为在聚合根的基础上进行操作

###  

### 实体

> 1、有唯一的标识，不受状态属性的影响。
>
> 2、可变性特征，状态信息一直可以变化。

**自我理解** 	 拥有唯一标示 ，状态是可以改变的，独立于属性，

面向对象中的对象  拥有自己的行为

### 值对象

> 1、它描述了领域中的一个东西
>
> 2、可以作为一个不变量。
>
> 3、当它被改变时，可以用另一个值对象替换。
>
> 4、可以和别的值对象进行相等性比较。

**自我理解**  无动作的对象 没有状态的改变的对象   实体中的属性都算值对象  值对象无法修改只能替换

**不要共享值对象**  

[DDD分层架构之值对象（介绍篇）](https://www.cnblogs.com/Leo_wl/p/4122147.html)

**聚合表中，可以根据层次关系命名列名**

1. **嵌入值**模式

   > 值对象  可以用做实体的外属性    在代码中添加外键  但是在数据库中依旧是一张表    
   >
   > 相当于将  数据库中的表拆分 为两个 类    一个作为实体  另一个 为值对象  
   >
   > 比如    用户的  住址 信息 就可以作为 值对象分离到另外一个类中 

2. **序列化大对象**模式

   > 将值对象序列化为 json 存储到实体属性中   
   >
   > 将值对象序列化为 byte 存储到实体属性中   





### 实体与值对象的区别：

> 1. 实体拥有标识，而值对象没有。
> 2. 相等性测试方式不同。实体根据标识判等，而值对象根据内部所有属性值判等。
> 3. 实体允许变化，值对象不允许变化。
> 4. 持久化的映射方式不同。实体采用单表继承、类表继承和具体表继承来映射类层次结构，而值对象使用嵌入值或序列化大对象方式映射。
