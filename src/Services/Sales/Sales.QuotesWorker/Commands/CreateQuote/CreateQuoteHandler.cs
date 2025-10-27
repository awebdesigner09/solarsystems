using BuildingBlocks.CQRS;
using MassTransit;
using Sales.Application.Data;
using Sales.Application.Dtos;
using Sales.Application.Exceptions;
using Sales.Domain.Enums;
using Sales.Domain.Models;
using Sales.Domain.ValueObjects;

namespace Sales.QuotesWorker.Commands.CreateQuote
{
    public class CreateQuoteHandler(
        IApplicationDbContext dbContext, IQuoteRepository quoteRepository
        ) : ICommandHandler<CreateQuoteCommand, CreateQuoteResult>
    {
        public async Task<CreateQuoteResult> Handle(CreateQuoteCommand command, CancellationToken cancellationToken)
        {
            var newQuote = await CreateNewQuote(command.QuoteRequest,cancellationToken);
            // Store Quote in DB and Cache
            var quote =await quoteRepository.StoreQuoteAsync(newQuote, cancellationToken);
            
            var quoteRequestId = QuoteRequestId.Of(command.QuoteRequest.Id);
            var quoteRequest = await dbContext.QuoteRequests.FindAsync([quoteRequestId], cancellationToken);
            quoteRequest?.UpdateStatus(QuoteRequestStatus.Ready);
            
            await dbContext.SaveChangesAsync(cancellationToken);
            return new CreateQuoteResult(quote.Id.Value);
        }
        private async Task<Quote> CreateNewQuote(QuoteRequestDto quoteRequestDto, CancellationToken cancellationToken)
        {
            var quoteRequestId = QuoteRequestId.Of(quoteRequestDto.Id);
            var quoteRequest = await dbContext.QuoteRequests.FindAsync([quoteRequestId], cancellationToken);
            if(quoteRequest is null)
            {
                throw new QuoteRequestNotFoundException(quoteRequestDto.Id);
            }
            var systemModelId = SystemModelId.Of(quoteRequestDto.SystemModelId);
            var SystemModel = await dbContext.SystemModels.FindAsync([systemModelId], cancellationToken);
            
            if(SystemModel is null)
            {
                throw new SystemModelNotFoundException(quoteRequest.SystemModelId.Value);
            }
            
            var customerId = CustomerId.Of(quoteRequestDto.CustomerId);
            var customer = await dbContext.Customers.FindAsync([customerId], cancellationToken);
            if(customer is null)
            {
                throw new CustomerNotFoundException(quoteRequest.CustomerId.Value);
            }
            decimal baseprice = SystemModel.BasePrice * 100;
            decimal tax1 = (SystemModel.BasePrice * 100) * 0.2m;
            decimal tax2 = (SystemModel.BasePrice * 100) * 0.1m;
            decimal totalprice = baseprice + tax1 + tax2;

            var newQuote=Quote.Create(
                id: QuoteId.Of(Guid.NewGuid()),
                quoteRequestId: QuoteRequestId.Of(quoteRequestDto.Id),
                validUntil: DateTime.Now.AddDays(30),
                components: Components.Of(noOfPanels:100,noOfMoutingSystems:100,noOfInverters:1,noOfBatteries:10),
                basePrice: baseprice,
                tax1: tax1,
                tax2: tax2,
                totalPrice: totalprice
                );
            return newQuote;
        }
    }
}
