
--Create db transactional entity that stores products information

CREATE TABLE products(
PrimaryKey INT NOT NULL PRIMARY KEY IDENTITY,
ProductName VARCHAR(100) NOT NULL,
SkuReference VARCHAR(150) NOT NULL UNIQUE,
CostCenterId VARCHAR(20) NULL,
ProductDescription VARCHAR(150) NULL,
CreationTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO products (ProductName, SkuReference, CostCenterId, ProductDescription)
VALUES
('Sample Product 1', 'US897722', '1', 'Sample description product 1'),
('Sample Product 2', 'US991100', '1', 'Sample description product 2'),
('Sample Product 3', 'UK122199', '5', 'Sample description product 3'),
('Sample Product 4', 'GER133229', '1', 'Sample description product 4'),
('Sample Product 5', 'CAN1449988', '3', 'Sample description product 5'),
('Sample Product 6', 'UK1337799', '3', 'Sample description product 6');