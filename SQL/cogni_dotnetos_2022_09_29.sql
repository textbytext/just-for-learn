declare @products table
(
    id INT INDEX IX_id CLUSTERED,
    title varchar(MAX)
);

declare @orders table
(
    id INT INDEX IX_id CLUSTERED,
    title varchar(MAX)
);

declare @order_products table
(
    order_id INT INDEX IX_oid NONCLUSTERED,
    product_id INT INDEX IX_pid NONCLUSTERED,
    count INT,
    INDEX IX_oid_pid NONCLUSTERED(order_id, product_id)
);

INSERT INTO @products (id, title)
VALUES (1, 'p1'),
    (2, 'p2'),
    (3, 'p3'),
    (4, 'p3'),
    (5, 'p3');

INSERT INTO @orders (id, title)
VALUES (1, 'o1'),
    (2, 'o2'),
    (3, 'o3'),
    (4, 'o4');

INSERT INTO @order_products (order_id, product_id, [count])
VALUES (1, 1, 4),
(1, 2, 3),
(2, 1, 1),
(2, 5, 10),
(3, 2, 4),
(3, 3, 2),
(3, 4, 67),
(3, 5, 17),
(4, 20, 1000);

-- cte
if (1 != 1)
begin
    ;with cte_op_full AS
    (
        select
            order_id = op.order_id
            ,product_id = op.product_id
            ,count = op.[count]
            ,product_title = p.title

        from @order_products as op
        left join @products p on op.product_id = p.id
    )
    select *
    from cte_op_full
    order by
        order_id asc,
        product_id asc
end;

-- cross apply
if (1 = 1)
begin
    select 
        o.id
        ,order_title = o.title
        ,op.product_id
        ,op.[count]
        ,product_title = p.title
    from @orders as o
    cross apply (select top(2) * from @order_products where order_id = o.id order by [count] desc) op
    join @products p on op.product_id = p.id
end;

-- outer apply
if (1 != 1)
BEGIN
    select 
        o.id
        ,order_title = o.title
        ,op.product_id
        ,op.[count]
        ,product_title = p.title
    from @orders as o
    outer apply (select top(2) * from @order_products where order_id = o.id order by [count] desc) op
    left outer join @products p on op.product_id = p.id
end;

