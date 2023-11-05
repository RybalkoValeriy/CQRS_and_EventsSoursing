http://localhost:5020/api/v1/newPost

- [ ] update libs to fresh
- [x] return some test data some test data for
- [ ] add seeding db by data
- [ ] swagger is not working for Query service
- [x] to make friendly ui and backend api
- [ ] update REST (type: list, "message":"txt") for UI

- [ ] add error handling for consumer part
- [ ] add author topics user
- [ ] moved from EF to Dapper
- [ ] REST no content vs empty list

migrations

- add:
  CQRS_and_EventsSoursing\SM-Post\Post.Query\Post.Query.Infrastructure\DataAccess>
  dotnet ef migrations add Init --project ..\..\Post.Query.Infrastructure --context DatabaseContext
- update:
  CQRS_and_EventsSoursing\SM-Post\Post.Query\Post.Query.Infrastructure\DataAccess>
  dotnet ef database update --project ..\..\Post.Query.Infrastructure --context DatabaseContext

    - ?Polling vs **SSE** vs WebSocket 